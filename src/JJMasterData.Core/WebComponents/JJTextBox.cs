﻿using JJMasterData.Commons.Language;
using JJMasterData.Core.Html;

namespace JJMasterData.Core.WebComponents;

public class JJTextBox : JJBaseControl
{
    public InputType InputType { get; set; }

    public int NumberOfDecimalPlaces { get; set; }

    public float? MinValue { get; set; }

    public float? MaxValue { get; set; }

    public JJTextBox()
    {
        InputType = InputType.Text;
        Visible = true;
        Enable = true;
    }

    internal override HtmlElement GetHtmlElement()
    {
        string inputType = InputType.ToString().ToLower();
        if (NumberOfDecimalPlaces > 0)
        {
            inputType = "text";
            CssClass += " jjdecimal";
        }

        var html = new HtmlElement(HtmlTag.Input)
            .WithNameAndId(Name)
            .WithAttributes(Attributes)
            .WithAttributeIf(!string.IsNullOrWhiteSpace(PlaceHolder),"placeholder",PlaceHolder)
            .WithAttribute("type", inputType)
            .WithCssClass("form-control")
            .WithCssClass(CssClass)
            .WithToolTip(Translate.Key(ToolTip))
            .WithAttributeIf(MaxLength > 0, "maxlength", MaxLength.ToString())
            .WithAttributeIf(NumberOfDecimalPlaces > 0, "jjdecimalplaces", NumberOfDecimalPlaces.ToString())
            .WithAttributeIf(InputType == InputType.Number, "onkeypress", "return jjutil.justNumber(event);")
            .WithAttributeIf(MinValue != null, "min", MinValue?.ToString())
            .WithAttributeIf(MaxValue != null, "max", MaxValue?.ToString())
            .WithAttributeIf(!string.IsNullOrEmpty(Text), "value", Text)
            .WithAttributeIf(ReadOnly, "readonly", "readonly")
            .WithAttributeIf(!Enable, "disabled", "disabled");

        return html;
    }

}
