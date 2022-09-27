﻿using JJMasterData.Core.WebComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace JJMasterData.Core.Html;

/// <summary>
/// Implementation of HTML element.
/// </summary>
public class HtmlElement
{
    private readonly Dictionary<string, string> _attributes;
    private readonly string _rawText;
    private readonly bool _hasRawText;
    private readonly HtmlElementsCollection _children;
    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlElement"/> class.
    /// </summary>
    internal HtmlElement()
    {
        _attributes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        _children = new HtmlElementsCollection();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlElement"/> class.
    /// </summary>
    /// <param name="rawText"></param>
    internal HtmlElement(string rawText) : this()
    {
        _rawText = rawText;
        _hasRawText = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlElement"/> class.
    /// </summary>
    internal HtmlElement(HtmlTag tag) : this()
    {
        Tag = new HtmlElementTag(tag);
    }

    /// <summary>
    /// Tag of the current element.
    /// </summary>
    public HtmlElementTag Tag { get; private set; }

    /// <summary>
    /// Insert HTML element as a child of caller element.
    /// </summary>
    public HtmlElement AppendElement(HtmlElement element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element));

        _children.Add(element);
        return this;
    }


    /// <summary>
    /// Insert HTML element as a child of caller element.
    /// </summary>
    public HtmlElement AppendElement(HtmlTag tag, Action<HtmlElement> elementAction)
    {
        var childElement = new HtmlElement(tag);
        elementAction.Invoke(childElement);
        _children.Add(childElement);
        return this;
    }

    /// <summary>
    /// Conditional insert HTML element as a child of caller element.
    /// </summary>
    public HtmlElement AppendElementIf(bool condition, HtmlTag tag, Action<HtmlElement> elementAction)
    {
        if (condition)
            AppendElement(tag, elementAction);

        return this;
    }

    /// <summary>
    /// Insert raw text as a child of caller element.
    /// </summary>
    /// <param name="rawText"></param>
    /// <returns></returns>
    public HtmlElement AppendText(string rawText)
    {
        var childElement = new HtmlElement(rawText);
        _children.Add(childElement);
        return this;
    }


    /// <summary>
    /// Conditional insert raw text as a child of caller element.
    /// </summary>
    public HtmlElement AppendTextIf(bool condition, string rawText)
    {
        if (condition)
            AppendText(rawText);

        return this;
    }

    /// <summary>
    /// Set HTML element name and ID.
    /// </summary>
    public HtmlElement WithNameAndId(string id)
    {
        if (!string.IsNullOrWhiteSpace(id))
            WithAttribute("id", id).WithAttribute("name", id);

        return this;
    }

    /// <summary>
    /// Set attribute to the HTML element.
    /// </summary>
    public HtmlElement WithAttribute(string name, string value)
    {
        _attributes.Add(name, value);
        return this;
    }




    /// <summary>
    /// Set Title to the HTML element.
    /// </summary>
    public HtmlElement WithToolTip(string tooltip)
    {
        if (!string.IsNullOrEmpty(tooltip))
        {
            if (_attributes.ContainsKey("title"))
                _attributes["title"] = tooltip;
            else
                _attributes.Add("title", tooltip);

            if (_attributes.ContainsKey(BootstrapHelper.DataToggle))
                _attributes[BootstrapHelper.DataToggle] = "tooltip";
            else
                _attributes.Add(BootstrapHelper.DataToggle, "tooltip");
        }

        return this;
    }


    /// <summary>
    /// Set attribute to the HTML element on condition.
    /// </summary>
    public HtmlElement WithAttributeIf(bool condition, string name, string value)
    {

        if (condition)
            _attributes.Add(name, value);

        return this;
    }


    /// <summary>
    /// Set classes attributes, if already exists will be ignored.
    /// </summary>
    public HtmlElement WithCssClass(string classes)
    {
        if (string.IsNullOrWhiteSpace(classes))
            return this;

        if (!_attributes.ContainsKey("class"))
            return WithAttribute("class", classes);

        var listClass = new List<string>();
        listClass.AddRange(_attributes["class"].Split(' '));
        foreach (string cssClass in classes.Split(' '))
        {
            if (!listClass.Contains(cssClass))
                listClass.Add(cssClass);
        }

        _attributes["class"] = string.Join(" ", listClass);

        return this;
    }


    /// <summary>
    /// Conditional to set classes attributes, if already exists will be ignored.
    /// </summary>
    public HtmlElement WithCssClassIf(bool conditional, string classes)
    {
        if (conditional)
            WithCssClass(classes);

        return this;
    }

    /// <summary>
    /// Set custom data attribute to HTML element.
    /// </summary>
    public HtmlElement WithDataAttribute(string name, string value)
    {
        return WithAttribute($"data-{name}", value);
    }

    /// <summary>
    /// Set range of attrs
    /// </summary>
    internal HtmlElement WithAttributes(Hashtable attributes)
    {
        foreach (DictionaryEntry v in attributes)
        {
            _attributes.Add(v.Key.ToString(), v.Value.ToString());
        }

        return this;
    }


    /// <summary>
    /// Gets current element HTML.
    /// </summary>
    internal string GetElementHtml(int tabCount)
    {
        string indentedText = string.Empty;
        if (tabCount > 0)
            indentedText = "\r\n" + new string('\t', tabCount);

        if (_hasRawText || Tag == null)
        {
            return string.Format("{0}{1}", indentedText, _rawText);
        }
            
        string tagLayout = Tag.HasClosingTag ? "{0}<{1}{2}>{{0}}{0}</{1}>" : "{0}<{1}{2}/>";
        string elementLayout = string.Format(tagLayout,
            indentedText,
            Tag.TagName.ToString().ToLower(),
            GetHtmlAttrs());

        if (!Tag.HasClosingTag)
            return elementLayout;

        return string.Format(elementLayout, GetElementContent(tabCount));
    }

    private string GetElementContent(int tabCount)
    {
        if (tabCount > 0)
            tabCount++;
        var content = new StringBuilder();
        foreach (var child in _children)
        {
            content.Append(child.GetElementHtml(tabCount));
        }

        return content.ToString();
    }

    private string GetHtmlAttrs()
    {
        var attrs = new StringBuilder();
        foreach (var item in _attributes)
        {
            attrs.AppendFormat(" {0}=\"{1}\"", item.Key, item.Value);
        }

        return attrs.ToString();
    }


}
