﻿module FSharp.Data.Validation.Samples.Primitives

open System
open System.Text.RegularExpressions
open System.Linq

open FSharp.Data.Validation

type UsernameFailures = 
    | Empty

type EmailAddressFailures = 
    | InvalidFormat

type PhoneNumberFailures = 
    | Empty
    | NumericStringOnly
    | InvalidFormat

type ZipCodeFailures = 
    | Empty
    | NumericStringOnly
    | InvalidFormat

type MyFailures =
    | RequiredField
    | InvalidUsername of UsernameFailures
    | InvalidEmailAddress of EmailAddressFailures
    | InvalidPhoneNumber of PhoneNumberFailures
    | InvalidZipCode of ZipCodeFailures
    | EmailAddressMatchesUsername

// app specific types
type Username = private Username of string

let mkUsername s =
    validation {
        withValue s
        disputeWithFact UsernameFailures.Empty isNotNull
        qed Username
    } |> fromVCtx

type EmailAddress = private EmailAddress of string

let mkEmailAddress s =
    validation {
        withValue s
        disputeWithFact EmailAddressFailures.InvalidFormat (fun s -> Regex.IsMatch(s, "[a-zA-Z0-9+._-]+@[a-zA-Z-]+\\.[a-z]+"))
        qed EmailAddress
    } |> fromVCtx

type PhoneNumber = private PhoneNumber of string

let private isNumericString (str:string) = 
    str.All(fun c -> Char.IsNumber(c))

let mkPhoneNumber s =
    validation {
        withValue s
        disputeWithFact PhoneNumberFailures.Empty isNotNull
        disputeWithFact PhoneNumberFailures.NumericStringOnly isNumericString
        disputeWithFact PhoneNumberFailures.InvalidFormat (isLength 10)
        qed PhoneNumber
    } |> fromVCtx

type ContactPreference =
    | Email
    | Phone

type ZipCode = private ZipCode of string

let mkZipCode s =
    validation {
        withValue s
        disputeWithFact ZipCodeFailures.Empty isNotNull
        disputeWithFact ZipCodeFailures.NumericStringOnly isNumericString
        disputeWithFact ZipCodeFailures.InvalidFormat (isLength 5)
        qed ZipCode
    } |> fromVCtx
