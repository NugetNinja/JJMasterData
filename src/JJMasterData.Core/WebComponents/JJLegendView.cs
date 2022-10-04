﻿using System;
using System.Linq;
using System.Text;
using JJMasterData.Commons.Dao;
using JJMasterData.Commons.Language;
using JJMasterData.Core.DataDictionary;
using JJMasterData.Core.Html;

namespace JJMasterData.Core.WebComponents;

public class JJLegendView : JJBaseView
{
    /// <summary>
    /// Pre-defined Form settings.
    /// </summary>
    public FormElement FormElement { get; set; }

    public bool ShowAsModal { get; set; }

    #region "Constructors"

    public JJLegendView(string elementName)
    {
        if (string.IsNullOrEmpty(elementName))
            throw new ArgumentNullException(nameof(elementName), "Nome do dicionário nao pode ser vazio");

        var dicParser = GetDictionary(elementName);
        FormElement = dicParser.GetFormElement();
        DoConstructor();
    }

    public JJLegendView(FormElement formElement)
    {
        FormElement = formElement;
        DoConstructor();
    }

    public JJLegendView(FormElement formElement, IDataAccess dataAccess) : base(dataAccess)
    {
        FormElement = formElement;
        DoConstructor();
    }

    private void DoConstructor()
    {
        Name = "iconLegend";
        ShowAsModal = false;
    }

    #endregion
    
    internal override HtmlElement GetHtmlElement()
    {
        if (ShowAsModal)
        {
            return GetHtmlModal();
        }

        var field = GetFieldLegend();
        return GetHtmlLegend(field);
    }

    private HtmlElement GetHtmlLegend(FormElementField field)
    {
        var div = new HtmlElement(HtmlTag.Div);

        if (field != null)
        {
            var cbo = new JJComboBox(DataAccess)
            {
                Name = field.Name,
                DataItem = field.DataItem
            };
            
            var values = cbo.GetValues();
            
            if (values is { Count: > 0 })
            {
                foreach (var item in values)
                {
                    div.AppendElement(HtmlTag.Div, div =>
                    {
                        div.WithAttribute("style", "height:40px");

                        div.AppendElement(new JJIcon(item.Icon, item.ImageColor, item.Description)
                        {
                            CssClass = "fa-fw fa-2x"
                        }.GetHtmlElement());
                        div.AppendText("&nbsp;&nbsp;");
                        div.AppendText(Translate.Key(item.Description));
                        div.AppendElement(HtmlTag.Br);
                    });
                }
            }
        }
        else
        {
            div.AppendElement(HtmlTag.Br);
            div.AppendText(Translate.Key("There is no caption to be displayed"));
        }

        return div;
    }

    private HtmlElement GetHtmlModal()
    {
        var field = GetFieldLegend();

        var form = new HtmlElement(HtmlTag.Div)
            .WithCssClass("form-horizontal")
            .WithAttribute("role", "form")
            .AppendElement(GetHtmlLegend(field));
        
        var dialog = new JJModalDialog
        {
            Name = Name,
            Title = Translate.Key("Information"),
            HtmlContent = form.GetElementHtml()
        };
        
        //TODO: Change this after finishing JJModalDialog
        return new HtmlElement(dialog.GetHtml());
    }
    
    private FormElementField GetFieldLegend()
    {
        return FormElement.Fields.FirstOrDefault(f 
            => f.Component == FormComponent.ComboBox && f.DataItem.ShowImageLegend);
    }

}
