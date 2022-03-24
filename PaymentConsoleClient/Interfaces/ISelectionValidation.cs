using System.Collections.Generic;

namespace PaymentConsoleClient.Interfaces;

public interface ISelectionValidation
{
    Dictionary<object, string> LimitMainMenuOptions();
    Dictionary<object, string> LimitUserAccountMenuOptions();
    Dictionary<object, string> LimitSavingsAccountMenuOptions();   

    int RestrictInputToPositiveInt();
    double RestrictInputToDouble();
}