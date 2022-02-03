using StarRepo.Domain;
using System.Drawing;

namespace StarRepo.Data
{
    /// <summary>
    /// Manages images.
    /// </summary>
    public class ImageHandler
    {
        private readonly string _imagesPath = string.Empty;

        public ImageHandler(string path, bool createIfNotExists)
        {
            if (createIfNotExists && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            _imagesPath = path;
        }

        /// <summary>
        /// Gets the file extension without the period.
        /// </summary>
        /// <param name="path">The path to extract from.</param>
        /// <returns>The extension.</returns>
        public static string GetFileExtension(string path) =>
            Path.GetExtension(path)[1..].ToLowerInvariant();

        /// <summary>
        /// Gets the normalized filenames.
        /// </summary>
        /// <param name="observationId">The id of the observation.</param>
        /// <param name="ext">The extension.</param>
        /// <returns>The filenames.</returns>
        public (string full, string thumb) GetFilenames(Guid observationId, string ext) =>
            (
                Path.Combine(_imagesPath, $"{observationId}_full.{ext}"), 
                Path.Combine(_imagesPath, $"{observationId}_thumb.{ext}"));
                    
        /// <summary>
        /// Stages the file and creates a thumbnail.
        /// </summary>
        /// <param name="observationId">The id of the observation.</param>
        /// <param name="path">The path to the source file.</param>
        /// <returns>The extension.</returns>
        public string ProcessForObservation(Guid observationId, string path)
        {
            var ext = GetFileExtension(path);
            var (filename, thumbname) = GetFilenames(observationId, ext);
            File.Copy(path, filename, true);

#pragma warning disable CA1416 // Validate platform compatibility
            var image = Image.FromFile(filename);
            var ratio = image.Width / 256;
            int height = image.Height / ratio;
            var thumb = image.GetThumbnailImage(256, height, null, IntPtr.Zero);
            thumb.Save(thumbname);
#pragma warning restore CA1416 // Validate platform compatibility

            return ext;
        }

        /// <summary>
        /// Gets the bytes to display for a file.
        /// </summary>
        /// <param name="obs">The observation.</param>
        /// <param name="thumbnail">A value indicating whether to fetch the full image or the thumbnail.</param>
        /// <returns>The bytes for the image.</returns>
        public async Task<byte[]> GetFileForObservation(Observation obs, bool thumbnail = false)
        {
            var (full, thumb) = GetFilenames(obs.Id, obs.Extension ?? string.Empty);
            return await File.ReadAllBytesAsync(thumbnail ? thumb : full);
        }            
    }
}
