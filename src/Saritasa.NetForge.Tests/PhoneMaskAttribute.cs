using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Saritasa.NetForge.Tests;

/// <summary>
/// Validated as a phone number with a specific mask pattern.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
internal class PhoneMaskAttribute : ValidationAttribute
{
    // Internal field to hold the maskPattern value.

    /// <summary>
    /// Mask pattern for the phone number validation.
    /// </summary>
    public string Mask { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mask">The mask pattern to be used for phone number validation.</param>
    public PhoneMaskAttribute(string mask)
    {
        Mask = mask;
    }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        var phoneNumber = (string)value!;
        var result = MatchesMask(Mask, phoneNumber);
        return result;
    }

    // Checks if the entered phone number matches the maskPattern.
    internal bool MatchesMask(string maskPattern, string phoneNumber)
    {
        if (maskPattern.Length != phoneNumber.Trim().Length)
        {
            // Length mismatch.
            return false;
        }
        for (var i = 0; i < maskPattern.Length; i++)
        {
            if (maskPattern[i] == 'd' && char.IsDigit(phoneNumber[i]) == false)
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
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Mask);
    }
}
