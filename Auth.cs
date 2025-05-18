using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Auth
{
    private static readonly string keyUrl = "https://raw.githubusercontent.com/apexkaustav/modmenu-auth/refs/heads/main/keys.json";

    public static async Task<bool> IsKeyValid(string userKey)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string json = await client.GetStringAsync(keyUrl);
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    var root = doc.RootElement;
                    var keys = root.GetProperty("valid_keys").EnumerateArray();

                    foreach (var key in keys)
                    {
                        if (key.GetString() == userKey)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error connecting to authentication server:\n" + ex.Message, "SN.Community.Ninja", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return false;
    }

    public static async Task ValidateUser(string userKey)
    {
        bool valid = await IsKeyValid(userKey);
        if (!valid)
        {
            MessageBox.Show(
                "Looks like, You Don't have the access of the modmenu...\nFetching the Data and sending it to the Server Admin...\n\nContact Discord : Secret Neighbor Community. / .therealking ( SN Ninja)",
                "Access Denied - SN.Community.Ninja",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );

            Environment.Exit(0); // close mod if key is invalid
        }
    }
}
