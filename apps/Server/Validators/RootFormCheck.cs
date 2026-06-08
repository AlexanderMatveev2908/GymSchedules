using System.ComponentModel.DataAnnotations;
using Server.LibNS;

namespace Server.ValidatorsNS.FormNS;

public static class RootFormCheck
{
  public static IResult? Check<T>(T dto) where T : notnull
  {
    List<ValidationResult> errors = [];

    bool isValid = Validator.TryValidateObject(
      dto,
      new ValidationContext(dto),
      errors,
      validateAllProperties: true
    );

    if (!isValid)
    {
      var formattedErrors = errors.Select(err => new
      {
        Field = err.MemberNames.FirstOrDefault(),
        Message = err.ErrorMessage
      });

      return Res.Json(400, "Validation failed", new
      {
        errors = formattedErrors
      });
    }

    return null;
  }
}