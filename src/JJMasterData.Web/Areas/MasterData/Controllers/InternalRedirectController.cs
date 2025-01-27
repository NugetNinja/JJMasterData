﻿using System.Collections;
using System.Data.SqlClient;
using System.Web;
using JJMasterData.Commons.Dao.Entity;
using JJMasterData.Commons.Exceptions;
using JJMasterData.Commons.Util;
using JJMasterData.Core.DataDictionary;
using JJMasterData.Core.WebComponents;
using JJMasterData.Web.Controllers;
using JJMasterData.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace JJMasterData.Web.Areas.MasterData.Controllers;

[Area("MasterData")]
public class InternalRedirectController : MasterDataController
{
    private string? _dictionaryName;
    private RelationType _relationType;
    private Hashtable? _relationValues;

    public ActionResult Index(string parameters)
    {
        LoadParameters(parameters);
        var userId = HttpContext.GetUserId();

        switch (_relationType)
        {
            case RelationType.List:
                {
                    var form = new JJFormView(_dictionaryName)
                    {
                        RelationValues = _relationValues
                    };

                    if (userId != null)
                    {
                        form.SetUserValues("USERID", userId);
                        form.SetCurrentFilter("USERID", userId);
                    }

                    ViewBag.HtmlPage = form.GetHtml();
                    ViewBag.ShowToolBar = false;
                    break;
                }
            case RelationType.View:
                {
                    var painel = new JJDataPanel(_dictionaryName)
                    {
                        PageState = PageState.View
                    };
                    if (userId != null)
                        painel.SetUserValues("USERID", userId);

                    painel.LoadValuesFromPK(_relationValues);

                    ViewBag.HtmlPage = painel.GetHtml();
                    ViewBag.ShowToolBar = false;
                    break;
                }
            case RelationType.Update:
                {
                    var painel = new JJDataPanel(_dictionaryName)
                    {
                        PageState = PageState.Update
                    };
                    if (userId != null)
                        painel.SetUserValues("USERID", userId);

                    painel.LoadValuesFromPK(_relationValues);

                    ViewBag.HtmlPage = painel.GetHtml();
                    ViewBag.ShowToolBar = true;
                    break;
                }
            default:
                throw new ArgumentOutOfRangeException();
        }

        return View();
    }

    [HttpPost]
    public ActionResult Save(string parameters)
    {
        LoadParameters(parameters);

        var userId = HttpContext.GetUserId();

        var painel = new JJDataPanel(_dictionaryName)
        {
            PageState = PageState.Update
        };

        painel.LoadValuesFromPK(_relationValues);
        if (userId != null)
            painel.SetUserValues("USERID", userId.ToString());

        var values = painel.GetFormValues();
        var errors = painel.ValidateFields(values, PageState.Update);
        var formElement = painel.FormElement;
        try
        {
            if (errors.Count == 0)
            {
                painel.Factory.SetValues(formElement, values);
            }
        }
        catch (SqlException ex)
        {
            errors.Add("DB", ExceptionManager.GetMessage(ex));
        }

        if (errors.Count > 0)
        {
            ViewBag.Error = new JJValidationSummary(errors).GetHtml();
            ViewBag.Success = false;
        }
        else
        {
            ViewBag.Success = true;
        }

        return View("Index");
    }

    private void LoadParameters(string parameters)
    {
        if (string.IsNullOrEmpty(parameters))
            throw new ArgumentNullException();

        _dictionaryName = null;
        _relationType = RelationType.List;
        _relationValues = new Hashtable();
        var @params = HttpUtility.ParseQueryString(Cript.EnigmaDecryptRP(parameters));
        _dictionaryName = @params.Get("formname");
        foreach (string key in @params)
        {
            switch (key.ToLower())
            {
                case "formname":
                    _dictionaryName = @params.Get(key);
                    break;
                case "viewtype":
                    _relationType = (RelationType)int.Parse(@params.Get(key));
                    break;
                default:
                    _relationValues.Add(key, @params.Get(key));
                    break;
            }
        }
    }
}