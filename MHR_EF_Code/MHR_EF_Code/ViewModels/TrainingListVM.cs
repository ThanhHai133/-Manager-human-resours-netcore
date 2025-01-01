using MHR_EF_Code.Models.Entities;


namespace MHR_EF_Code.ViewModels
{
    public class TrainingListVM
    {
        public List<Training> Trainings { get; set; }
        public List<Guid> JoinedTrainings { get; set; }
    }
}
