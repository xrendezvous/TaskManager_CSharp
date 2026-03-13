using TaskManager.Services.DTO;

namespace TaskManager.Services.Interface
{
    public interface IProjectService
    {
        List<ProjectListDto> GetProjectsForList();
        ProjectDetailsDto GetProjectDetails(int projectId);
    }
}
