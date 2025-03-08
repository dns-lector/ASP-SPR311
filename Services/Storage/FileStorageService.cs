
namespace ASP_SPR311.Services.Storage
{
    public class FileStorageService : IStorageService
    {
        private const String storagePath = "C:\\storage\\ASP311\\";

        public string GetRealPath(string name)
        {
            return storagePath + name;
        }

        public string SaveFile(IFormFile formFile)
        {
            // 1. З імені файлу визначити розширення
            // 2. згенерувати нове ім'я зберігши розширення, переконатись в його унікальності
            // 3. скопіювати formFile до сховища під новим іменем
            var ext = Path.GetExtension(formFile.FileName);
            String savedName;
            String fullName;
            do
            {
                savedName = Guid.NewGuid() + ext;
                fullName = storagePath + savedName;
            } while (File.Exists(fullName));

            formFile.CopyTo(new FileStream(fullName, FileMode.CreateNew));

            return savedName;
        }
    }
}
