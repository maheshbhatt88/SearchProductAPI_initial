
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Infrastructure;
//using Infrastructure.Models;


//namespace ApplicationLayer
//{

//    public class SaveSearchHistoryCommand : IRequest<bool>
//    {
       
//        public int? UserId { get; set; }

//        public string? Query { get; set; }

//        public DateTime? Timestamp { get; set; }
//    }
//    public class SaveSearchHistoryCommandHandler: IRequestHandler<SaveSearchHistoryCommand, bool>
//    {
//        private readonly IGenericRepository<Infrastructure.Models.SearchHistory> _repositorygen;

//        public SaveSearchHistoryCommandHandler(IGenericRepository<Infrastructure.Models.SearchHistory> repositorygen)
//        {
//            _repositorygen = repositorygen;

//        }
//        public async Task<bool> Handle(SaveSearchHistoryCommand command,CancellationToken ct)
//        {
//            var searchHistory = new SearchHistory
//            {
//                UserId = command.UserId,
//                Query = command.Query,
//                Timestamp = command.Timestamp
//            };
//            await _repositorygen.Add(searchHistory);
//            await _repositorygen.SaveChangesAsync();
//            return true;
//        }
//    }

//}
