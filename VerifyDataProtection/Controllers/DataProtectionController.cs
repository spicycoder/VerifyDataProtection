using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace VerifyDataProtection.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataProtectionController(IDataProtectionProvider dataProtectionProvider) : ControllerBase
{
    private const string Purpose = "test-purpose";

    [HttpGet("encrypt")]
    public IActionResult Encrypt(string input)
    {
        var protector = dataProtectionProvider.CreateProtector(Purpose);
        var encrypted = protector.Protect(input);
        return Ok(encrypted);
    }

    [HttpGet("decrypt")]
    public IActionResult Decrypt(string encryptedInput)
    {
        var protector = dataProtectionProvider.CreateProtector(Purpose);
        var decrypted = protector.Unprotect(encryptedInput);
        return Ok(decrypted);
    }
}
