using HSEApiTraining.Models.BannedPhone;
using System.Collections.Generic;

namespace HSEApiTraining
{
    public interface IBanService
    {
        (IEnumerable<BannedPhone> BannedPhones, string Error) GetBannedPhones();
        string AddBannedPhone(AddBannedPhoneRequest request);
        string DeleteBannedPhone(int id);
        string DeleteAllBannedPhones();
    }

    public class BanService : IBanService
    {
        private IBanRepository _banRepository;
        public BanService(IBanRepository banRepository)
            => _banRepository = banRepository;

        public (IEnumerable<BannedPhone> BannedPhones, string Error) GetBannedPhones()
            => _banRepository.GetBannedPhones();

        public string AddBannedPhone(AddBannedPhoneRequest request)
            => _banRepository.AddBannedPhone(request);

        public string DeleteBannedPhone(int id)
            => _banRepository.DeleteBannedPhone(id);

        public string DeleteAllBannedPhones()
            => _banRepository.DeleteAllBannedPhones();
    }
}
