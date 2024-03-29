﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Helpers
{
    public static class Util
    {
        public static string ConvertPhoneForStorage(string phone)
        {
            string output = phone.Trim();

            if (output.Contains('('))
                output = output.Remove(output.IndexOf('('), 1);
            if (output.Contains(')'))
                output = output.Remove(output.IndexOf(')'), 1);

            // remove dash
            while (output.Contains('-'))
            {
                output = output.Remove(output.IndexOf('-'), 1);
            }

            // remove dot
            while (output.Contains('.'))
            {
                output = output.Remove(output.IndexOf('.'), 1);
            }

            // remove space
            while (output.Contains(' '))
            {
                output = output.Remove(output.IndexOf(' '), 1);
            }

            return output.ToLower();
        }


        public static string ConvertPhoneForDisplay(string phone)
        {
            string output = phone;
            if (output.Contains('x'))
                output = output.Insert(10, " ");
            output = output.Insert(6, "-");
            output = output.Insert(3, " ");
            output = output.Insert(3, ")");
            output = "(" + output;

            return output;
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static void MapValidationErrors(Dictionary<string, string> dic, ModelStateDictionary ms)
        {
            foreach (var error in dic)
            {
                ms.AddModelError(error.Key, error.Value);
            }
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static IEnumerable<SelectListItem> CreateSelectListFromEnum(Type enumType)
        {

            return (from object item in Enum.GetValues(enumType)
                    let fi = enumType.GetField(item.ToString())
                    let attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault()
                    let title = attribute == null ? item.ToString() : ((DescriptionAttribute)attribute).Description
                    select new SelectListItem
                    {
                        Value = item.ToString(),
                        Text = title
                    }).ToList();


        }

        public static IEnumerable<SelectListItem> CreateSelectListFromEnum(Type enumType, string selectedItem)
        {

            return (from object item in Enum.GetValues(enumType)
                    let fi = enumType.GetField(item.ToString())
                    let attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault()
                    let title = attribute == null ? item.ToString() : ((DescriptionAttribute)attribute).Description
                    select new SelectListItem
                    {
                        Value = item.ToString(),
                        Text = title,
                        Selected = selectedItem == item.ToString()
                    }).ToList();


        }
    }

    public enum ManageMessageId
    {
        ChangePasswordSuccess,
        SetPasswordSuccess,
        RemoveSignInSuccess,
        ResetPasswordSuccess,
        ChangeCompanyInfoSuccess,
        ChangeEmailSuccess,
        ChangeProfileSuccess,
        NewDelegateSuccess,
        PublishSuccess,
        PublishFail,
        UnpublishSuccess,
        UnpublishFail
    }
}