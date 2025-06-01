namespace FishingPlanner.Services
{
    public class FishingForecastService
    {
        public async Task<string> GetFishingForecastAsync(DateTime date, double latitude, double longitude)
        {
            // TODO: заменить на реальный вызов API  
            //await Task.Delay(50);

            return (date.Day % 3) switch
            {
                0 => "Высокая активность",
                1 => "Средняя активность",
                _ => "Низкая активность"
            };
        }
    }
}