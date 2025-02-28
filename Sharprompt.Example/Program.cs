﻿using System;
using System.Text;
using System.Text.Json;

using Sharprompt.Example.Models;

using static Sharprompt.Enums;

namespace Sharprompt.Example;

// ReSharper disable LocalizableElement
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Prompt.SkillLevel = Prompt.Select<SkillLevel>("Choose your skills", defaultValue: SkillLevel.Advanced);

            var type = Prompt.Select<ExampleType>("Choose prompt example", defaultValue: ExampleType.Input);

            switch (type)
            {
                case ExampleType.Input:
                    RunInputSample();
                    break;
                case ExampleType.InputWithDefaultValueTabToReset:
                    RunInputSampleWithDefaultValueTabToReset();
                    break;
                case ExampleType.InputWithDefaultValueTabToSelect:
                    RunInputSampleWithDefaultValueTabToSelect();
                    break;
                case ExampleType.Confirm:
                    RunConfirmSample();
                    break;
                case ExampleType.Password:
                    RunPasswordSample();
                    break;
                case ExampleType.Select:
                    RunSelectSample();
                    break;
                case ExampleType.MultiSelect:
                    RunMultiSelectSample();
                    break;
                case ExampleType.SelectWithEnum:
                    RunSelectEnumSample();
                    break;
                case ExampleType.MultiSelectWithEnum:
                    RunMultiSelectEnumSample();
                    break;
                case ExampleType.List:
                    RunListSample();
                    break;
                case ExampleType.Bind:
                    RunBindSample();
                    break;
                case ExampleType.BindProperty:
                    RunBindPropertySample();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private static void RunInputSample()
    {
        var name = Prompt.Input<string>("What's your name?", defaultValue: "John Smith", placeholder: "At least 3 characters", validators: [Validators.Required(), Validators.MinLength(3)]);
        Console.WriteLine($"Hello, {name}!");
    }
    private static void RunInputSampleWithDefaultValueTabToReset()
    {
        var colour = Prompt.Input<string>("What's your favourite colour?", defaultValue: "blue", DefaultValueTabBehaviour.TabToReset);
        Console.WriteLine($"Your answer is: {colour}!");
    }

    private static void RunInputSampleWithDefaultValueTabToSelect()
    {
        var colour = Prompt.Input<string>("What's your favourite colour?", defaultValue: "blue", DefaultValueTabBehaviour.TabToSelect);
        Console.WriteLine($"Your answer is: {colour}!");
    }

    private static void RunConfirmSample()
    {
        var answer = Prompt.Confirm("Are you ready?");
        Console.WriteLine($"Your answer is {answer}");
    }

    private static void RunPasswordSample()
    {
        var secret = Prompt.Password("Type new password", placeholder: "At least 8 characters", validators: [Validators.Required(), Validators.MinLength(8)]);
        Console.WriteLine($"Password OK, {secret}");
    }

    private static void RunSelectSample()
    {
        var city = Prompt.Select("Select your city", ["Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai"], pageSize: 3);
        Console.WriteLine($"Hello, {city}!");
    }

    private static void RunMultiSelectSample()
    {
        var options = Prompt.MultiSelect("Which cities would you like to visit?", ["Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai"], pageSize: 3, defaultValues: ["Tokyo"]);
        Console.WriteLine($"You picked {string.Join(", ", options)}");
    }

    private static void RunSelectEnumSample()
    {
        var value = Prompt.Select<MyEnum>("Select enum value", defaultValue: MyEnum.Bar);
        Console.WriteLine($"You selected {value}");
    }

    private static void RunMultiSelectEnumSample()
    {
        var value = Prompt.MultiSelect<MyEnum>("Select enum value", defaultValues: [MyEnum.Bar]);
        Console.WriteLine($"You picked {string.Join(", ", value)}");
    }

    private static void RunListSample()
    {
        var value = Prompt.List<string>("Please add item(s)");
        Console.WriteLine($"You picked {string.Join(", ", value)}");
    }

    private static void RunBindSample()
    {
        var model = Prompt.Bind<MyFormModel>();
        Console.WriteLine($"Forms OK, {JsonSerializer.Serialize(model)}");
    }

    private static void RunBindPropertySample()
    {
        var model = new MyFormModel();

        var result = Prompt.BindProperty(model, nameof(model.Name));

        Console.WriteLine($"Forms OK, {JsonSerializer.Serialize(model)}");
    }
}
