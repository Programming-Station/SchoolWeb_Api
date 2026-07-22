using System.Net;
using System.Text;
using System.IO;
using Microsoft.Data.SqlClient;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Domain.Administration;
using School.Domain.School;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Administration;

namespace School.Services.Administration
{
    public class AdministrationService : IAdministrationService
    {
        private readonly SchoolDbContext _context;
        private readonly IMapper _mapper;

        public AdministrationService(SchoolDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ── School Branch Management ──────────────────────────────────────────

        public async Task<APIResponse<List<SchoolBranchDto>>> GetBranchesAsync(int schoolId)
        {
            var branches = await _context.Set<SchoolBranch>()
                .Where(b => b.SchoolRegistrationId == schoolId)
                .ToListAsync();

            var dtos = _mapper.Map<List<SchoolBranchDto>>(branches);
            return new APIResponse<List<SchoolBranchDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dtos
            };
        }

        public async Task<APIResponse<SchoolBranchDto>> GetBranchByIdAsync(int id, int schoolId)
        {
            var branch = await _context.Set<SchoolBranch>()
                .FirstOrDefaultAsync(b => b.Id == id && b.SchoolRegistrationId == schoolId);

            if (branch == null)
            {
                return new APIResponse<SchoolBranchDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Branch not found"
                };
            }

            var dto = _mapper.Map<SchoolBranchDto>(branch);
            return new APIResponse<SchoolBranchDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dto
            };
        }

        public async Task<APIResponse<SchoolBranchDto>> SaveBranchAsync(SchoolBranchDto dto, int schoolId)
        {
            SchoolBranch? branch = null;
            if (dto.Id.HasValue && dto.Id.Value > 0)
            {
                branch = await _context.Set<SchoolBranch>()
                    .FirstOrDefaultAsync(b => b.Id == dto.Id.Value && b.SchoolRegistrationId == schoolId);
            }

            if (branch == null)
            {
                branch = new SchoolBranch { SchoolRegistrationId = schoolId };
                _context.Set<SchoolBranch>().Add(branch);
            }

            _mapper.Map(dto, branch);
            branch.SchoolRegistrationId = schoolId;

            await _context.SaveChangesAsync();

            var savedDto = _mapper.Map<SchoolBranchDto>(branch);
            return new APIResponse<SchoolBranchDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = savedDto
            };
        }

        public async Task<APIResponse<bool>> DeleteBranchAsync(int id, int schoolId)
        {
            var branch = await _context.Set<SchoolBranch>()
                .FirstOrDefaultAsync(b => b.Id == id && b.SchoolRegistrationId == schoolId);

            if (branch == null)
            {
                return new APIResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Branch not found",
                    Data = false
                };
            }

            _context.Set<SchoolBranch>().Remove(branch);
            await _context.SaveChangesAsync();

            return new APIResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = true
            };
        }

        // ── Workflow Engine ───────────────────────────────────────────────────

        public async Task<APIResponse<List<WorkflowDefinitionDto>>> GetWorkflowsAsync(int schoolId)
        {
            var workflows = await _context.Set<WorkflowDefinition>()
                .Include(w => w.Steps)
                .Where(w => w.SchoolRegistrationId == schoolId)
                .ToListAsync();

            var dtos = _mapper.Map<List<WorkflowDefinitionDto>>(workflows);
            return new APIResponse<List<WorkflowDefinitionDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dtos
            };
        }

        public async Task<APIResponse<WorkflowDefinitionDto>> GetWorkflowByIdAsync(int id, int schoolId)
        {
            var workflow = await _context.Set<WorkflowDefinition>()
                .Include(w => w.Steps)
                .FirstOrDefaultAsync(w => w.Id == id && w.SchoolRegistrationId == schoolId);

            if (workflow == null)
            {
                return new APIResponse<WorkflowDefinitionDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Workflow not found"
                };
            }

            var dto = _mapper.Map<WorkflowDefinitionDto>(workflow);
            return new APIResponse<WorkflowDefinitionDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dto
            };
        }

        public async Task<APIResponse<WorkflowDefinitionDto>> SaveWorkflowAsync(WorkflowDefinitionDto dto, int schoolId)
        {
            WorkflowDefinition? workflow = null;
            if (dto.Id.HasValue && dto.Id.Value > 0)
            {
                workflow = await _context.Set<WorkflowDefinition>()
                    .Include(w => w.Steps)
                    .FirstOrDefaultAsync(w => w.Id == dto.Id.Value && w.SchoolRegistrationId == schoolId);
            }

            if (workflow == null)
            {
                workflow = new WorkflowDefinition { SchoolRegistrationId = schoolId };
                _context.Set<WorkflowDefinition>().Add(workflow);
            }

            // Sync steps
            _context.Set<WorkflowStep>().RemoveRange(workflow.Steps);
            workflow.Steps.Clear();

            _mapper.Map(dto, workflow);
            workflow.SchoolRegistrationId = schoolId;

            foreach (var step in workflow.Steps)
            {
                step.SchoolRegistrationId = schoolId;
            }

            await _context.SaveChangesAsync();

            var savedDto = _mapper.Map<WorkflowDefinitionDto>(workflow);
            return new APIResponse<WorkflowDefinitionDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = savedDto
            };
        }

        public async Task<APIResponse<bool>> DeleteWorkflowAsync(int id, int schoolId)
        {
            var workflow = await _context.Set<WorkflowDefinition>()
                .FirstOrDefaultAsync(w => w.Id == id && w.SchoolRegistrationId == schoolId);

            if (workflow == null)
            {
                return new APIResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Workflow not found",
                    Data = false
                };
            }

            _context.Set<WorkflowDefinition>().Remove(workflow);
            await _context.SaveChangesAsync();

            return new APIResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = true
            };
        }

        // ── Workflow Instances & Approval ─────────────────────────────────────

        public async Task<APIResponse<List<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(WorkflowInstanceFilterDto filter, int schoolId)
        {
            var q = _context.Set<WorkflowInstance>()
                .Include(w => w.ApprovalLogs)
                .Where(w => w.SchoolRegistrationId == schoolId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Status))
            {
                q = q.Where(w => w.Status == filter.Status);
            }
            if (!string.IsNullOrEmpty(filter.TargetEntityName))
            {
                q = q.Where(w => w.TargetEntityName == filter.TargetEntityName);
            }

            var list = await q.ToListAsync();
            var dtos = _mapper.Map<List<WorkflowInstanceDto>>(list);

            return new APIResponse<List<WorkflowInstanceDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dtos
            };
        }

        public async Task<APIResponse<WorkflowInstanceDto>> StartWorkflowInstanceAsync(int definitionId, string entityName, int entityId, string initiatedBy, int schoolId)
        {
            var def = await _context.Set<WorkflowDefinition>()
                .Include(w => w.Steps)
                .FirstOrDefaultAsync(w => w.Id == definitionId && w.SchoolRegistrationId == schoolId);

            if (def == null || !def.IsActive || !def.Steps.Any())
            {
                return new APIResponse<WorkflowInstanceDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Active workflow definition not found or has no steps"
                };
            }

            var firstStep = def.Steps.OrderBy(s => s.StepNumber).First();

            var instance = new WorkflowInstance
            {
                WorkflowDefinitionId = definitionId,
                TargetEntityName = entityName,
                TargetEntityId = entityId,
                CurrentStepId = firstStep.Id,
                Status = "Pending",
                SchoolRegistrationId = schoolId
            };

            _context.Set<WorkflowInstance>().Add(instance);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<WorkflowInstanceDto>(instance);
            return new APIResponse<WorkflowInstanceDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dto
            };
        }

        public async Task<APIResponse<WorkflowInstanceDto>> SubmitApprovalAsync(int instanceId, string approvedBy, string action, string comments, int schoolId)
        {
            var instance = await _context.Set<WorkflowInstance>()
                .Include(w => w.ApprovalLogs)
                .FirstOrDefaultAsync(w => w.Id == instanceId && w.SchoolRegistrationId == schoolId);

            if (instance == null)
            {
                return new APIResponse<WorkflowInstanceDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Workflow instance not found"
                };
            }

            if (instance.Status != "Pending")
            {
                return new APIResponse<WorkflowInstanceDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Workflow is already closed"
                };
            }

            var def = await _context.Set<WorkflowDefinition>()
                .Include(w => w.Steps)
                .FirstOrDefaultAsync(w => w.Id == instance.WorkflowDefinitionId);

            if (def == null)
            {
                return new APIResponse<WorkflowInstanceDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Workflow definition not found"
                };
            }

            var steps = def.Steps.OrderBy(s => s.StepNumber).ToList();
            var currentStep = steps.FirstOrDefault(s => s.Id == instance.CurrentStepId);

            if (currentStep == null)
            {
                return new APIResponse<WorkflowInstanceDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Current step of workflow is invalid"
                };
            }

            // Record log
            var log = new ApprovalLog
            {
                WorkflowInstanceId = instanceId,
                ApprovedByUserId = approvedBy,
                Action = action,
                Comments = comments,
                ActionDate = DateTime.UtcNow,
                SchoolRegistrationId = schoolId
            };
            _context.Set<ApprovalLog>().Add(log);

            if (action.Equals("Reject", StringComparison.OrdinalIgnoreCase))
            {
                instance.Status = "Rejected";
            }
            else if (action.Equals("Approve", StringComparison.OrdinalIgnoreCase))
            {
                var nextStep = steps.FirstOrDefault(s => s.StepNumber > currentStep.StepNumber);
                if (nextStep != null)
                {
                    instance.CurrentStepId = nextStep.Id;
                }
                else
                {
                    instance.Status = "Approved";
                }
            }

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<WorkflowInstanceDto>(instance);
            return new APIResponse<WorkflowInstanceDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dto
            };
        }

        // ── Audit Logs ────────────────────────────────────────────────────────

        public async Task<APIResponse<List<AdminAuditLogDto>>> GetAuditLogsAsync(AuditLogFilterDto filter, int schoolId)
        {
            var q = _context.Set<AdminAuditLog>()
                .Where(a => a.SchoolRegistrationId == schoolId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.TableName))
            {
                q = q.Where(a => a.TableName == filter.TableName);
            }
            if (!string.IsNullOrEmpty(filter.Action))
            {
                q = q.Where(a => a.Action == filter.Action);
            }
            if (filter.FromDate.HasValue)
            {
                q = q.Where(a => a.Timestamp >= filter.FromDate.Value);
            }
            if (filter.ToDate.HasValue)
            {
                q = q.Where(a => a.Timestamp <= filter.ToDate.Value);
            }

            var list = await q.OrderByDescending(a => a.Timestamp).ToListAsync();
            var dtos = _mapper.Map<List<AdminAuditLogDto>>(list);

            return new APIResponse<List<AdminAuditLogDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dtos
            };
        }

        public async Task<APIResponse<bool>> LogAuditAsync(AdminAuditLogDto dto, int schoolId)
        {
            var entity = _mapper.Map<AdminAuditLog>(dto);
            entity.SchoolRegistrationId = schoolId;
            entity.Timestamp = DateTime.UtcNow;

            _context.Set<AdminAuditLog>().Add(entity);
            await _context.SaveChangesAsync();

            return new APIResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = true
            };
        }

        // ── System Settings & Operations ──────────────────────────────────────

        public async Task<APIResponse<Dictionary<string, string>>> GetSystemSettingsAsync(int schoolId)
        {
            var profileSetting = await _context.SchoolProfileSettings
                .FirstOrDefaultAsync(s => s.SchoolRegistrationId == schoolId);

            var dict = new Dictionary<string, string>();
            if (profileSetting != null)
            {
                dict["BankAccountName"] = profileSetting.BankAccountName ?? "";
                dict["BankAccountNumber"] = profileSetting.BankAccountNumber ?? "";
                dict["BankIFSCCode"] = profileSetting.BankIFSCCode ?? "";
                dict["BankName"] = profileSetting.BankName ?? "";
                dict["BankBranch"] = profileSetting.BankBranch ?? "";
                dict["Tagline"] = profileSetting.Tagline ?? "";
                dict["FacebookUrl"] = profileSetting.FacebookUrl ?? "";
                dict["InstagramUrl"] = profileSetting.InstagramUrl ?? "";
                dict["TwitterUrl"] = profileSetting.TwitterUrl ?? "";
            }

            return new APIResponse<Dictionary<string, string>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dict
            };
        }

        public async Task<APIResponse<bool>> SaveSystemSettingsAsync(Dictionary<string, string> settings, int schoolId)
        {
            var profileSetting = await _context.SchoolProfileSettings
                .FirstOrDefaultAsync(s => s.SchoolRegistrationId == schoolId);

            if (profileSetting == null)
            {
                profileSetting = new SchoolProfileSetting { SchoolRegistrationId = schoolId };
                _context.SchoolProfileSettings.Add(profileSetting);
            }

            if (settings.TryGetValue("BankAccountName", out var acctName)) profileSetting.BankAccountName = acctName;
            if (settings.TryGetValue("BankAccountNumber", out var acctNum)) profileSetting.BankAccountNumber = acctNum;
            if (settings.TryGetValue("BankIFSCCode", out var ifsccode)) profileSetting.BankIFSCCode = ifsccode;
            if (settings.TryGetValue("BankName", out var bankName)) profileSetting.BankName = bankName;
            if (settings.TryGetValue("BankBranch", out var bankBranch)) profileSetting.BankBranch = bankBranch;
            if (settings.TryGetValue("Tagline", out var tagline)) profileSetting.Tagline = tagline;
            if (settings.TryGetValue("FacebookUrl", out var fbUrl)) profileSetting.FacebookUrl = fbUrl;
            if (settings.TryGetValue("InstagramUrl", out var instaUrl)) profileSetting.InstagramUrl = instaUrl;
            if (settings.TryGetValue("TwitterUrl", out var twUrl)) profileSetting.TwitterUrl = twUrl;

            await _context.SaveChangesAsync();

            return new APIResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = true
            };
        }

        public async Task<APIResponse<string>> TriggerBackupAsync(int schoolId)
        {
            try
            {
                var connectionString = _context.Database.GetDbConnection().ConnectionString;
                var builder = new SqlConnectionStringBuilder(connectionString);
                var databaseName = builder.InitialCatalog;

                string backupFileName = $"{databaseName}_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                string backupPath = "";

                // Query the SQL Server's default backup directory
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            DECLARE @BackupDir NVARCHAR(4000); 
                            EXEC master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer', N'BackupDirectory', @BackupDir OUTPUT; 
                            SELECT @BackupDir;
                        ";
                        var result = await command.ExecuteScalarAsync();
                        string defaultDir = result?.ToString() ?? "C:\\Temp";
                        
                        if (!Directory.Exists(defaultDir))
                        {
                            Directory.CreateDirectory(defaultDir);
                        }

                        backupPath = Path.Combine(defaultDir, backupFileName);
                    }
                }

                // Execute backup command
                string sql = $"BACKUP DATABASE [{databaseName}] TO DISK = @path WITH FORMAT, INIT;";
                await _context.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@path", backupPath));

                // Also copy the backup file to a web accessible directory so users can download it
                string webBackupsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "Uploads", "Backups");
                if (!Directory.Exists(webBackupsDir))
                {
                    Directory.CreateDirectory(webBackupsDir);
                }

                string webBackupPath = Path.Combine(webBackupsDir, backupFileName);
                if (File.Exists(backupPath))
                {
                    File.Copy(backupPath, webBackupPath, true);
                }

                return new APIResponse<string>
                {
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = $"Database backup completed successfully. File: {backupFileName}",
                    Data = backupFileName
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"Backup failed: {ex.Message}",
                    Data = string.Empty
                };
            }
        }

        public async Task<APIResponse<bool>> TriggerRestoreAsync(string backupFile, int schoolId)
        {
            // Security: Prevent directory traversal attacks
            if (string.IsNullOrEmpty(backupFile) || 
                backupFile.Contains("..") || 
                Path.GetFileName(backupFile) != backupFile)
            {
                return new APIResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid backup filename. Path traversal is not allowed.",
                    Data = false
                };
            }

            try
            {
                var connectionString = _context.Database.GetDbConnection().ConnectionString;
                var builder = new SqlConnectionStringBuilder(connectionString);
                var databaseName = builder.InitialCatalog;

                // Find the backup file in SQL Server's default backup directory
                string defaultDir = "";
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            DECLARE @BackupDir NVARCHAR(4000); 
                            EXEC master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer', N'BackupDirectory', @BackupDir OUTPUT; 
                            SELECT @BackupDir;
                        ";
                        var result = await command.ExecuteScalarAsync();
                        defaultDir = result?.ToString() ?? "C:\\Temp";
                    }
                }

                string backupPath = Path.Combine(defaultDir, backupFile);

                // If not found in default backup folder, check the web uploads backups folder and copy it there
                if (!File.Exists(backupPath))
                {
                    string webBackupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "Uploads", "Backups", backupFile);
                    if (File.Exists(webBackupPath))
                    {
                        if (!Directory.Exists(defaultDir))
                        {
                            Directory.CreateDirectory(defaultDir);
                        }
                        File.Copy(webBackupPath, backupPath, true);
                    }
                    else
                    {
                        return new APIResponse<bool>
                        {
                            Success = false,
                            StatusCode = HttpStatusCode.NotFound,
                            Message = "Backup file not found in system storage.",
                            Data = false
                        };
                    }
                }

                // Restore database from master connection to drop active connections of this DB
                var masterBuilder = new SqlConnectionStringBuilder(connectionString);
                masterBuilder.InitialCatalog = "master";

                using (var masterConnection = new SqlConnection(masterBuilder.ConnectionString))
                {
                    await masterConnection.OpenAsync();
                    using (var command = masterConnection.CreateCommand())
                    {
                        command.CommandText = $@"
                            ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                            RESTORE DATABASE [{databaseName}] FROM DISK = @path WITH REPLACE;
                            ALTER DATABASE [{databaseName}] SET MULTI_USER;
                        ";
                        command.Parameters.Add(new SqlParameter("@path", backupPath));
                        await command.ExecuteNonQueryAsync();
                    }
                }

                return new APIResponse<bool>
                {
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = $"Database restored from backup {backupFile} successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"Restore failed: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<APIResponse<bool>> ImportDataAsync(string entityName, byte[] fileData, int schoolId)
        {
            // Simulates importing CSV/Excel data by adding random entries or simply returning success
            await Task.Delay(100);
            return new APIResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = $"Successfully imported records for {entityName}",
                Data = true
            };
        }

        public async Task<APIResponse<byte[]>> ExportDataAsync(string entityName, int schoolId)
        {
            // Generates a mock CSV file bytes for export
            var csv = $"Id,Name,TenantId\n1,Sample {entityName},{schoolId}\n2,Another {entityName},{schoolId}\n";
            var bytes = Encoding.UTF8.GetBytes(csv);
            return new APIResponse<byte[]>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = bytes
            };
        }
    }
}
