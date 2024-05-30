using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Saritasa.NetForge.Tests;

/// <summary>
/// Validated as a phone number with a specific mask pattern.
/// <b>Note:</b> It only use for the test custom validation only, not using on anywhere.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class PhoneMaskAttribute : ValidationAttribute
{
    /// <summary>
    /// Mask pattern for the phone number validation.
    /// </summary>
    private readonly string mask;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mask">The mask pattern to be used for phone number validation.</param>
    public PhoneMaskAttribute(string mask)
    {
        this.mask = mask;
    }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return false;
        }

        var phoneNumber = (string)value;
        var result = MatchesMask(mask, phoneNumber);
        return result;
    }

    /// <summary>
    /// Checks if the entered phone number matches the <paramref name="maskPattern"/>.
    /// </summary>
    private bool MatchesMask(string maskPattern, string phoneNumber)
    {
        if (maskPattern.Length != phoneNumber.Trim().Length)
        {
            // Length mismatch.
            return false;
        }
        for (var i = 0; i < maskPattern.Length; i++)
        {
            if (maskPattern[i] == 'd' && !char.IsDigit(phoneNumber[i]))
            {
                // Digit expected at this position.
                return false;
            }
            if (maskPattern[i] == '-' && phoneNumber[i] != '-')
            {
                // Spacing character expected at this position.
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc />
    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, mask);
    }
}
