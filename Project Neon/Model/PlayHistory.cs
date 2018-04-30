using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Project_Neon.Model
{
    public class PlayHistoryEntry
    {
        private string fileName { get; set; }
        private string filePath { get; set; }
        private TimeSpan position { get; set; }
        private TimeSpan duration { get; set; }
        private int percentage { get; set; }
        private StorageItemThumbnail thumbnail { get; set;}

        public PlayHistoryEntry(string fileName, string filePath, TimeSpan position, TimeSpan duration)
        {
            this.fileName = fileName;
            this.filePath = filePath;
            this.position = position;
            this.duration = duration;
            this.percentage = CalPercentage(position, duration);
        }

        private int CalPercentage(TimeSpan position, TimeSpan duration)
        {
            double percentage = position.TotalMinutes / duration.TotalMinutes;
            return (int)Math.Round(percentage, MidpointRounding.AwayFromZero);
        }

        public async Task<StorageItemThumbnail> GetMediaThumbnail(StorageFile file)
        {
            const uint requestedSize = 190;
            const ThumbnailMode thumbnailMode = ThumbnailMode.VideosView;
            const ThumbnailOptions thumbnailOptions = ThumbnailOptions.UseCurrentScale;
            return await file.GetThumbnailAsync(thumbnailMode, requestedSize, thumbnailOptions);
        }
    }

    public class PlayHistoryManager
    {
        private static ObservableCollection<PlayHistoryEntry> historyCollection = new ObservableCollection<PlayHistoryEntry>();

        public static void AddNewEntry(PlayHistoryEntry entry)
        {
            historyCollection.Insert(0, entry);
        }

        public static void RemoveEntry(int index)
        {
            historyCollection.RemoveAt(index);
        }

        public static void RemoveAllEntry()
        {
            historyCollection.Clear();
        }

        public static bool IsEmpty()
        {
            if (historyCollection.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static ObservableCollection<PlayHistoryEntry> GetHisCollection()
        {
            return historyCollection;
        }
    }
}
