using Microsoft.JSInterop;

namespace BWShared
{
    public static class FileUtil
    {
        public static ValueTask SaveAs(this IJSRuntime js, string filename, byte[] data)
            => js.InvokeVoidAsync("saveAsFile", filename, Convert.ToBase64String(data));
    }
}
