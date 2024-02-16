using NetMapper.Enums;
using System.Runtime.InteropServices;
using System.Text;

namespace NetMapper.Services.Helpers;
public static class ComImports
{

    [DllImport("mpr.dll")]
    internal static extern int WNetAddConnection2(
        ref NETRESOURCE oNetworkResource,
        string? sPassword,
        string? sUserName,
        int iFlags);

    [DllImport("mpr.dll")]
    internal static extern int WNetCancelConnection2(
        string sLocalName,
        uint iFlags,
        int iForce);
    /// <summary>
    /// Gets information about a given network resource.
    /// </summary>
    /// <param name="localName"></param>
    /// <param name="remoteName">A string builder that gets the remote name.</param>
    /// <param name="length">The size of the <paramref name="remoteName"/> buffer (input).
    /// If the maximum length is not big enough, this parameter also returns the
    /// the required buffer size.</param>
    /// <returns>Status flag. If the buffer was not big enough,   </returns>
    /// 
    [DllImport("mpr.dll", SetLastError = true)]

    internal static extern int WNetGetConnection(string localName, StringBuilder remoteName, ref int length);
}
