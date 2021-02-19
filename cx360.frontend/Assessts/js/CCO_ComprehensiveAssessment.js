﻿var parentClass = "";
var _age;
var sectionStatus;
var dataTableFamilyMembersFlg = false;
var editPermission="true", deletePermission="true";
$(document).ready(function () {

    BindDropDowns()
    CloseErrorMeeage();
    InitalizeDateControls();
    $('.bgStart').show();
    DisableSaveButtonChildSection();
    
    $('#GuardianshipAndAdvocacy').DataTable({
        "stateSave": true,
        "bDestroy": true,
        "paging": true,
        "searching": false,
        "autoWidth": false,
        'columnDefs': [
        { 'visible': false, 'targets': [5, 6, 7, 8, 9,10,11,12] }
        ]
    });
});
function InitalizeDateControls() {
    InitCalendar($(".date"), "date controls");
    $('.time').timepicker(getTimepickerOptions());
    $('.time').on("timeFormatError", function (e) { timepickerFormatError($(this).attr('id')); });
}
function BindDropDowns() {
    token = _token;
    reportedBy = _userId;

    $.ajax({
        type: "GET",
        url: 'https://staging-api.cx360.net/api/Incident/GetClientDetails',
        headers: {
            'TOKEN': token
        },
        success: function (result) {
            BindDropDownIndividualName(result);

        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}
function BindDropDownIndividualName(result) {
   
    $.each(result, function (data, value) {
        $("#DropDownClientId").append($("<option></option>").val(value.ClientID).html(value.LastName + "," + " " + value.FirstName));
    });
    $("#DropDownClientId").attr("josn", JSON.stringify(result))

    $("#DropDownClientId").attr("onchange", "FillClientDetails(this)");
    $("#DropDownClientId").attr("disabled", true);
    clientId = GetParameterValues('ClientId');

    if (clientId != undefined) {

        $("select[id$=DropDownClientId]").val(clientId);


        var jsonObject = $("select[id$=DropDownClientId]").attr("josn");
        var parse = jQuery.parseJSON(jsonObject);
        var res = $.grep(parse, function (IndividualNmae) {
            return IndividualNmae.ClientID == clientId;
        });

        var DBO = (res[0].BirthDate);
        if (DBO != null) {
            DBO = DBO.slice(0, 10).split('-');
            DBO = DBO[1] + '/' + DBO[2] + '/' + DBO[0];
        }


        $("#TextBoxDateofBirth").val(DBO)
        $("#TextBoxStreetAddress1").val(res[0].Address1)
        $("#TextBoxStreetAddress2").val(res[0].Address2)
        $("#TextBoxCity").val(res[0].City)
        $("#TextBoxState").val(res[0].State)
        $("#TextBoxZipCode").val(res[0].ZipCode)
        $('#TextBoxPhoneNumber').val(formatPhoneNumberClient(res[0].Phone));
        $("#TextBoxAge").val(res[0].Age)
        $("#TextBoxGender").val(res[0].Gender)
        $("#TextBoxEthnicity").val(res[0].Ethnicity)
        $("#TextBoxRace").val(res[0].Race)
        $("#TextBoxWillowbrookStatus").val(res[0].WillowBrook);
    }
    showHideFieldsBindOnStart();
}
function FillClientDetails(object) {

    var selectedValue = $(object).val();
    var jsonObject = $("#DropDownClientId").attr("josn");
    var parse = jQuery.parseJSON(jsonObject);
    var res = $.grep(parse, function (IndividualName) {
        return IndividualName.ClientID == selectedValue;
    });

}
//#region Save functions tab
function InsertModify(sectionName, _class, tabName) {

    if (!validateMasterSectionTab(sectionName)) return;
    className = parentClass;
    if ($("#Btn" + parentClass + "Ok").text() == "Edit") {
        $('.' + parentClass + ' .form-control').attr("disabled", false);
        $('.' + parentClass + ' input[type=radio]').prop("disabled", false);
        $("#Btn" + parentClass + "Ok").text("Ok");
        $(".greenColor").prop("disabled", false);
        $(".redColor").prop("disabled", false);
        return;
    }

    if (sectionName != 'masterSection') {

        doConfirm("Have you completed the section ?", function yes() {
            sectionStatus = "Completed";
            InsertModifySectionTabs(sectionName, _class, tabName);
        }, function no() {
            sectionStatus = "Inprogress"
            InsertModifySectionTabs(sectionName, _class, tabName);
            });

    } else
    {
        InsertModifySectionTabs(sectionName, _class, tabName);
    }


}

function InsertModifySectionTabs(sectionName, _class, tabName) {
   debugger;
    var json = [],
        item = {},
        tag;
    blankComprehensiveAssessmentId = $("#TextBoxCompAssessmentId").val();
    $('.' + sectionName + ' .' + _class).each(function () {
        tag = $(this).attr('id').replace("TextBox", "").replace("Checkbox", "").replace("DropDown", "").replace("Radio", "").replace("TextBox1", "");
        if ($(this).hasClass("required")) {
            if ($(this).val() == "") {
                item[tag] = $(this).val(-1);
            }
            else {
                item[tag] = $(this).val();
            }
        }
        else {
            if ($(this).attr("type") == "radio") {
                if ($(this).prop("checked") == true) item[tag] = $(this).val();
                else { }
            }
            else if ($(this).attr("type") == "checkbox") {
                if ($(this).prop("checked") == true) item[tag] = true;
                else {
                item[tag]=false;
                }
                }
            else {
                item[tag] = jsonWrapperWithTimePicker(tag, this);
            }
        }
    });
    item["CompanyId"] = "1";
    if (sectionName != 'masterSection') {
        item["CompAssessmentId"] = $("#TextBoxCompAssessmentId").val();
        item["CompAssessmentVersioningId"] = $("#TextBoxCompAssessmentVersioningId").val();
        item["Status"] = sectionStatus;
    }
    json.push(item);

    if (sectionName=='guardianshipAndAdvocacy') {
        json=[];       
        var oTable = $("#GuardianshipAndAdvocacy").DataTable().rows().data();
        $.each(oTable, function (index, value) {
            var itemBodyFirst = {};
            itemBodyFirst["CompAssessmentId"] = $("#TextBoxCompAssessmentId").val();
            itemBodyFirst["CompAssessmentVersioningId"] = $("#TextBoxCompAssessmentVersioningId").val();
            itemBodyFirst["Status"] = sectionStatus;
            itemBodyFirst["NoActiveGuardian"] = $("#CheckboxNoActiveGuardian").prop("checked") == true?1:0;
            itemBodyFirst["NotApplicableGuardian"] = $("#CheckboxNotApplicableGuardian").prop("checked") == true?1:0;
            itemBodyFirst["WhoHelpToMakeDecisionInLife"] = value[6] == undefined ? value.HelpToMakeDecisionInLife : value[6];
            itemBodyFirst["HowPersonHelpMemberMakeDecision"] = value[7] == undefined ? value.HowHelpToMakeDecision : value[7];
            itemBodyFirst["PersonInvolvementWithMember"] = value[8] == undefined ? value.PersonInvolvementWithMember : value[8];
            itemBodyFirst["HelpSignApproveLifePlan"] = value[9] == undefined ? value.HelpSignApproveLifePlan :  value[9];
            itemBodyFirst["HelpSignApproveMedical"] = value[10] == undefined ? value.HelpSignApproveMedical : value[10];
            itemBodyFirst["HelpSignApproveFinancial"] = value[11] == undefined ? value.HelpSignApproveFinancial : value[11];
            itemBodyFirst["Other"] = value[12] == undefined ? value.Other : value[12];
            itemBodyFirst["GuardianshipProof"] = value[13] == undefined ? value.GuardianshipProof : null;
            json.push(itemBodyFirst);
        });
    }
  
    $.ajax({
        type: "POST",
        data: { TabName: tabName, Json: JSON.stringify(json), ReportedBy: reportedBy },
        url: GetAPIEndPoints("INSERTMODIFYCOMPREHENIVEASSESSMENTDETAIL"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                ComprehensiveAssessmentSaved(result,sectionName);


            }
            else {
                showErrorMessage(result.Message);
            }

        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}

function validateMasterSectionTab(sectionName) {
    
    var checked = null; var checkBoxLength = 0;
    parentClass = sectionName;

    $("." + parentClass + " .req_feild").each(function () {
        console.log($(this).val());
        console.log($(this).attr("name"));
        if ($(this).is(":visible"))
        if (($(this).val() == "" || $(this).val() == "-1" )&& ($(this).attr("type") != "checkbox")  ) {
            console.log($(this));
            $(this).siblings("span.errorMessage").removeClass("hidden");
            $(this).focus();
            checked = false;
            return checked;
        }
        else if ($(this).attr("type") == "radio") {

            var element = $(this).attr("name");
            checkBoxLength = $('input[name=' + element + ']:checked').length;
            if ($('input[name=' + element + ']:checked').length == 0 && $(this).is(":visible")) {
                $(this).parent().parent().parent().next().children().removeClass("hidden");
                $(this).focus();
                checked = false;
                return checked;
            }

        }
        else if ($(this).attr("type") == "checkbox") {
            var element = $(this).attr("name");
            checkBoxLength = $('input[name=' + element + ']:checked').length;
            if ($('input[name=' + element + ']:checked').length == 0 && $(this).is(":visible")) {
                $(this).parent().parent().parent().next().children().removeClass("hidden");
                $(this).focus();
                checked = false;
                return checked;
            }

        }
    });
    if (checked == null) {
        return true;
    }
}
function CloseErrorMeeage() {

    $('select').click(function () {
        if ($(this).hasClass("req_feild") && ($(this).val() != '' || $(this).val() > 1)) {
            $(this).siblings("span.errorMessage").addClass("hidden");
        }
    });
    $('input').blur(function () {
        if (($(this).attr("type") == "radio" && $(this).hasClass("req_feild"))) {
            var radio = $(this).attr("name");
            $('input[name=' + radio + ']').change(function () {
                $(this).parent().parent().parent().next().children().addClass("hidden");

            })
        }
        else if ($(this).attr("type") == "checkbox" && $(this).hasClass("req_feild")) {
            var checkboxId = $(this).attr("name");
            $('input[name=' + checkboxId + ']').change(function () {
                $(this).parent().parent().parent().next().children().addClass("hidden");

            })
        }
        else if ($(this).hasClass("req_feild")) {
            $(this).siblings("span.errorMessage").addClass("hidden");

        }
    })
    $('textarea').blur(function () {
        $(this).siblings("span.errorMessage").addClass("hidden");
    })
    $('span').click(function () {
        $(this).siblings("span.errorMessage").addClass("hidden");
    })
}

function showFields(hideFieldClass) {
    $('.' + hideFieldClass).removeAttr('hidden');
    $('.' + hideFieldClass).show();
}

function hideFields(hideFieldClass) {
    $('.' + hideFieldClass).children().children().val("");
    $('.' + hideFieldClass).hide();
}

function uncheckedFields(hideFieldClass, check = false) {
    if (check == true) {
        var newClass = hideFieldClass.replace('Class', 'NoConcerns')
        $('.' + newClass).prop('checked', false);
    }
    else {
        var newClass = hideFieldClass.replace('Class', '')
        $('.' + newClass).prop('checked', false);
    }

}
//#region show hide controls
function ShowHideFields(current, type, hideFieldClass) {

    if (type == 'radio') {
        if ($(current).parent().text().trim() == 'Yes') {
            showFields(hideFieldClass);
            if (hideFieldClass == 'educationProgramsClass') {
                $("input[name=RadioMemPursuingAdditionalEducation]").prop('checked', false);
                $('.educationProgramsNoClass').hide();
            }
        }
        else if (hideFieldClass == 'fallenInLastMonthClass') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'diarrheaVomitingClass') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if ((hideFieldClass == 'constipationConcernsClass' || hideFieldClass == 'oralCareNeedsClass' || hideFieldClass == 'swallowingNeedsClass' || hideFieldClass == 'gerdClass' || hideFieldClass == 'supervisionNeedsClass') && $(current).parent().text().trim() != 'No concerns at this time') {
            showFields(hideFieldClass);
        }
        else if ((hideFieldClass == 'constipationConcernsClass' || hideFieldClass == 'oralCareNeedsClass' || hideFieldClass == 'gerdClass' || hideFieldClass == 'supervisionNeedsClass') && $(current).parent().text().trim() == 'No concerns at this time') {
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'swallowingNeedsClass' && $(current).parent().text().trim() == 'No concerns at this time') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if ((hideFieldClass == 'individualMedicaitonClass' || hideFieldClass == 'medication_s_Class') && $(current).parent().text().trim() != 'Never') {
            showFields(hideFieldClass);
        }
        else if (hideFieldClass == 'individualMedicaitonClass' && $(current).parent().text().trim() == 'Never') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'medication_s_Class' && $(current).parent().text().trim() == 'Never ') {
            hideFields(hideFieldClass);
        }
        else if ((hideFieldClass == 'publicTransportationClass' || hideFieldClass == 'budgetingMoneyClass') && ($(current).parent().text().trim() != 'Independent')) {
            showFields(hideFieldClass);
        }
        else if (hideFieldClass == 'publicTransportationClass' && $(current).parent().text().trim() == 'Independent ') {
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'administerMedicationsClass' && $(current).parent().text().trim() != 'Independent with taking medications at this time') {
            uncheckedFields(hideFieldClass);
            $('.administerMedicationsTextClass').removeAttr('hidden');
            $('.administerMedicationsTextClass').show();
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'administerMedicationsClass' && $(current).parent().text().trim() == 'Independent with taking medications at this time') {
            $(".administerMedicationsTextClass").hide();
            $('.administerMedicationsTextClass').children().children().val("");
            showFields(hideFieldClass);
        }
        else if (hideFieldClass == 'psychiatricSymptomsClass') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'behavioralSupportPlanClass') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
            $('.intrusiveInterventionsClass').hide();

        }
        else if (hideFieldClass == 'maintainSafetyClass') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'intrusiveInterventionsClass') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if ($(current).parent().text().trim() == 'No' && hideFieldClass == 'educationProgramsClass') {
            $("input[name=RadioCurreEducationMeetNeed]").prop('checked', false);
            $("input[name=RadioChooseCurrentEducation]").prop('checked', false);
            hideFields(hideFieldClass);
            $('.educationProgramsNoClass').removeAttr('hidden');
            $('.educationProgramsNoClass').show();
        }
        else if (hideFieldClass == 'educationProgramsClass') {
            $("input[name=RadioCurreEducationMeetNeed]").prop('checked', false);
            $("input[name=RadioChooseCurrentEducation]").prop('checked', false);
            $("input[name=RadioMemPursuingAdditionalEducation]").prop('checked', false);
            hideFields(hideFieldClass);
            $('.educationProgramsNoClass').hide();
        }
        else if (hideFieldClass == 'accessVRClass') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if ($(current).parent().text().trim() != 'No known history' && hideFieldClass == 'significantChallengingClass') {
            showFields(hideFieldClass);
        }
        else if (hideFieldClass == 'significantChallengingClass') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'memberCurrentEmploymentStatusClass' && $(current).parent().text().trim() != 'Retired' && $(current).parent().text().trim() != 'Not employed') {
            showFields(hideFieldClass);
        }
        else if (hideFieldClass == 'memberCurrentEmploymentStatusClass' && ($(current).parent().text().trim() == 'Retired' || $(current).parent().text().trim() == 'Not employed')) {
            $("input[name=RadioMemSatisfiedWithCurrentEmployer]").prop('checked', false);
            $("input[name=RadioMemPaycheck]").prop('checked', false);
            $("input[name=RadioDescMemEmploymentSetting]").prop('checked', false);
            $("input[name=RadioSatisfiedCurrentEmploymentSetting]").prop('checked', false);
            $("input[name=RadioMemWorkInIntegratedSetting]").prop('checked', false);
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'involvementInCriminalClass') {
            $("input[name=RadioMemCurrOnProbation]").prop('checked', false);
           // $("input[name=RadioMemInvolCriminalJusticeSystem]").prop('checked', false);
            $("input[name=RadioMemNeedLegalAid]").prop('checked', false);
            $("input[name=RadioCrimJustSystemImpactHousing]").prop('checked', false);
            $("input[name=RadioCrimJustSystemImpactEmployment]").prop('checked', false);
            $("#TextBoxExpInvolCriminalJusticeSystem").val("");
            $('.paroleOrProbationClass').hide();
            $('.paroleOrProbationClass').children().children().val("");
            $('.imapctHousingClass').hide();
            $('.imapctHousingClass').children().children().val("");
            $('.impactCurrentEmploymentClass').hide();
            $('.impactCurrentEmploymentClass').children().children().val("");
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'sexuallyActiveClass') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'mentalHealthConditionClass') {
            // uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'suicidalThoughtsBehaviorsClass') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'healthProfessionalSuicidalRiskClass') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'monitoredByPsychiatristClass') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else {
            hideFields(hideFieldClass);
        }


    }
    else if (type == 'radioNo') {
        if ($(current).parent().text().trim() == 'No' && hideFieldClass != 'satisfiedWithProviderClass') {
            showFields(hideFieldClass);
        }

        else if (hideFieldClass == 'selfDirectSupportsClass') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'satisfiedWithProviderClass' && $(current).parent().text().trim() != 'No') {
            showFields(hideFieldClass);
        }
        else if (hideFieldClass == 'satisfiedWithProviderClass' && $(current).parent().text().trim() == 'No') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else {
            hideFields(hideFieldClass);
        }
    }
    else if (type == 'radioWithName') {
        if (hideFieldClass == 'paroleOrProbationClass' && $(current).parent().text().trim() == 'Probation' || $(current).parent().text().trim() == 'Parole') {
            showFields(hideFieldClass);
        }
        else {
            hideFields(hideFieldClass);
        }
    }
    else if (type == 'dropdown') {

        if ($(current).children("option:selected").text() == 'Co Representation' && hideFieldClass == 'representationStatusClass') {
            $('.cabRepContact2').removeAttr('hidden');
            $('.cabRepContact1').removeAttr('hidden');
            $('.cabRepContact1').show();
            $('.cabRepContact2').show();
        }
        else if (hideFieldClass == 'representationStatusClass' && $(current).children("option:selected").text() == 'Full Representation') {
            $('.cabRepContact1').removeAttr('hidden');
            $('.cabRepContact1').show();
            $('.cabRepContact2').hide();
            $('.cabRepContact2').children().children().val("");
        }
        else if ($(current).children("option:selected").text() == 'No Representation' || $(current).children("option:selected").val() == '' && hideFieldClass == 'representationStatusClass') {
            $('.cabRepContact1').children().children().val("");
            $('.cabRepContact2').children().children().val("");
            $('.cabRepContact1').hide();
            $('.cabRepContact2').hide();
        }
        else if (hideFieldClass == 'memberMakeDecisionClass' && $(current).children("option:selected").text() == 'Legal Guardian') {
            showFields(hideFieldClass);
        }
        else if (hideFieldClass == 'memberMakeDecisionClass') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if (hideFieldClass == 'memberCurrentDiagnosesClass' && $(current).children("option:selected").text() != null && $(current).children("option:selected").val() > 0) {
            showFields(hideFieldClass);
        }
        else if (hideFieldClass == 'memberCurrentDiagnosesClass' && ($(current).children("option:selected").text() == null || $(current).children("option:selected").val() == "")) {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else {

        }
    }
    else if (type == 'text') {
        if (hideFieldClass == 'circleSupportContactClass') {
            if ($(current).val() != null && $(current).val() != '') {
                showFields(hideFieldClass);
            }
        }

        else if ($('#TextBoxNicknamePreferredName').text() == 'Professional') {
            showFields(hideFieldClass);
        }
        else if (hideFieldClass == 'genderClass') {
            if ($(current).text() == 'Female') {
                $('.femaleGenderClass').removeAttr('hidden');
                $('.femaleGenderClass').show();
            }
            else if ($(current).text() == 'Male') {
                $('.maleGenderClass').removeAttr('hidden');
                $('.maleGenderClass').show();
            }
            showFields(hideFieldClass);
        }
        else {
            hideFields(hideFieldClass);
        }
    }
    else if (type == 'checkbox') {
        if (($(current).prop("checked") && (hideFieldClass == 'mentalHealthServiceClass' || hideFieldClass == 'mentalHealthServiceOtherClass'))) {
            showFields(hideFieldClass);
        }
        else if ($(current).prop("checked") && hideFieldClass == 'individualsAges5Class') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        else if (($('input[name=CheckboxLongTermInpatientTreatment]:checked').length == 1 || $('input[name=CheckboxAcuteInpatientTreatment]:checked').length == 1) && hideFieldClass == 'acuteInpatientClass') {
            showFields(hideFieldClass);
        }
        else if ($('input[name=CheckboxindividualsAges5]:checked').length == 0 && _age <= 5) {
            showFields(hideFieldClass);
        }
        else if ((hideFieldClass == 'skinIntegrityClass' || hideFieldClass == 'nutritionalNeedsClass' || hideFieldClass == 'dentalOralCareNeedsClass') && $(current).parent().text().trim() != 'No concerns at this time') {
            var check = true;
            uncheckedFields(hideFieldClass, check);
            showFields(hideFieldClass);
        }
        else if ((hideFieldClass == 'skinIntegrityClass' || hideFieldClass == 'nutritionalNeedsClass' || hideFieldClass == 'dentalOralCareNeedsClass') && $(current).parent().text().trim() == 'No concerns at this time') {
            uncheckedFields(hideFieldClass);
            hideFields(hideFieldClass);
        }
        if (($(current).prop("checked") && (hideFieldClass == 'guardianAndApplicable'))) {
            if ($('#CheckboxNoActiveGuardian').prop("checked") && $('#CheckboxNotApplicableGuardian').prop("checked") ) {
                showFields(hideFieldClass);
                AddGuardianshipAndAdvocacy();
                $("#GuardianshipAndAdvocacy").val("");
            }
        }
        else {
            hideFields(hideFieldClass);
        }

    }

}

function showHideFieldsBindOnStart() {
   
    var hideFieldClass = 'willowbrookStatusClass';
    if ($('#TextBoxWillowbrookStatus').val() == 'true') {
        showFields(hideFieldClass);
    }

    if ($('#TextBoxCircleofSupportContact').val() != null) {
        $('.circleSupportContactClass').removeAttr('hidden');
        $('.circleSupportContactClass').show();
    }
    if ($('#TextBoxTypeofContact').val() == 'Professional') {
        $('.typeOfContactClass').removeAttr('hidden');
        $('.typeOfContactClass').show();
    }
    if ($('#TextBoxDateofBirth').val() != null && $('#TextBoxDateofBirth').val() != '') {
        var birthday = $('#TextBoxDateofBirth').val();
        var age = getAge(birthday);
        _age = age;
        if (age >= 45) {
            $('.ageClass').removeAttr('hidden');
            $('.ageClass').show();
        }
        if (age >= 65) {
            $('.ageOlder65Class').removeAttr('hidden');
            $('.ageOlder65Class').show();
        }
        if (age <= 19) {
            $('.ageLess18Class').removeAttr('hidden');
            $('.ageLess18Class').show();
        }
        if (age >= 5) {
            $('.ageLess5Class').removeAttr('hidden');
            $('.ageLess5Class').show();
        }


    }
    if ($('#TextBoxGender').val() == 'Female') {
        $('.femaleGenderClass').removeAttr('hidden');
        $('.femaleGenderClass').show();
    }
    if ($('#TextBoxGender').val() == 'Male') {
        $('.maleGenderClass').removeAttr('hidden');
        $('.maleGenderClass').show();
    }
}

function getAge(dateString) {
    let birth = new Date(dateString);
    let now = new Date();
    let beforeBirth = ((() => { birth.setDate(now.getDate()); birth.setMonth(now.getMonth()); return birth.getTime() })() < birth.getTime()) ? 0 : 1;
    return now.getFullYear() - birth.getFullYear() - beforeBirth;
}

function ComprehensiveAssessmentSaved(result,sectionName) {
  
    if (result.Success == true && result.IsException == false) {
        if (result.AllTabsComprehensiveAssessment[0].ValidatedRecord == false) {
            showErrorMessage("Comprehensvie Assessment already exists in Draft for client");
            return;
        }
        else {
            showRecordSaved("Record Saved Successfully.");
            $(".btnDisable").prop("disabled", false);
            $("#btnSaveAsNew").addClass("hidden");
            $("#btnPublishVersion").show();
            $("#btnPrintPDf").show();
            //changeExistingURL(result.AllTabsComprehensiveAssessment[0].ComprehensiveAssessmentId, result.AllTabsComprehensiveAssessment[0].AssessmentVersioningId);
            if (sectionName == 'masterSection') {
                $("#TextBoxCompAssessmentId").val(result.AllTabsComprehensiveAssessment[0].CompAssessmentId);
                $("#TextBoxCompAssessmentVersioningId").val(result.AllTabsComprehensiveAssessment[0].CompAssessmentVersioningId);
            }
           
            if (result.AllTabsComprehensiveAssessment[0].DocumentVersion != "") {
                $("#TextBoxDocumentStatus").text(result.AllTabsComprehensiveAssessment[0].DocumentStatus);
                $("#TextBoxDocumentVersion").text(result.AllTabsComprehensiveAssessment[0].DocumentVersion);
            }
            //$('.masterSection .comprehensiveAssessment').attr("disabled", true);
            //if ($("#ComprehensiveAssessmentSaveBtn").text() == "Ok") {
            //    $("#ComprehensiveAssessmentSaveBtn").text("Edit");
            //}
           
            if (result.AllTabsComprehensiveAssessment[0].EligibilityInformationId!=0 ) {

                $("#TextBoxEligibilityInformationId").val(result.AllTabsComprehensiveAssessment[0].EligibilityInformationId);

                if (result.AllTabsComprehensiveAssessment[0].Status != null) {
                    var status = result.AllTabsComprehensiveAssessment[0].Status;
                    if (status == "Completed") {
                        $("#statusCompletedEligibilityInformation").show();
                        $("#statusStartEligibilityInformation").hide();
                        $("#statusInprogressEligibilityInformation").hide();
                    }
                    else {
                        $("#statusCompletedEligibilityInformation").hide();
                        $("#statusStartEligibilityInformation").hide();
                        $("#statusInprogressEligibilityInformation").show();
                    }
                
                }
                
            }
            if (result.AllTabsComprehensiveAssessment[0].CommunicationId!=0) {

                $("#TextBoxCommunicationId").val(result.AllTabsComprehensiveAssessment[0].CommunicationId);

                if (result.AllTabsComprehensiveAssessment[0].Status != null) {
                    var status = result.AllTabsComprehensiveAssessment[0].Status;
                    if (status == "Completed") {
                        $("#statusCompletedCommunicationLanguage").show();
                        $("#statusStartCommunicationLanguage").hide();
                        $("#statusInprogressCommunicationLanguage").hide();
                    }
                    else {
                        $("#statusCompletedCommunicationLanguage").hide();
                        $("#statusStartCommunicationLanguage").hide();
                        $("#statusInprogressCommunicationLanguage").show();
                    }
                
                }
            }
            if (result.AllTabsComprehensiveAssessment[0].MemberProviderId!=0) {

                $("#TextBoxMemberProviderId").val(result.AllTabsComprehensiveAssessment[0].MemberProviderId);

                if (result.AllTabsComprehensiveAssessment[0].Status != null) {
                    var status = result.AllTabsComprehensiveAssessment[0].Status;
                    if (status == "Completed") {
                        $("#statusCompletedMemberProviders").show();
                        $("#statusStartMemberProviders").hide();
                        $("#statusInprogressMemberProviders").hide();
                    }
                    else {
                        $("#statusCompletedMemberProviders").hide();
                        $("#statusStartMemberProviders").hide();
                        $("#statusInprogressMemberProviders").show();
                    }
                
                }
            }
            if (result.AllTabsComprehensiveAssessment[0].GuardianshipAndAdvocacyId!=0) {

                $("#TextBoxGuardianshipAndAdvocacyId").val(result.AllTabsComprehensiveAssessment[0].GuardianshipAndAdvocacyId);

                if (result.AllTabsComprehensiveAssessment[0].Status != null) {
                    var status = result.AllTabsComprehensiveAssessment[0].Status;
                    if (status == "Completed") {
                        $("#statusCompletedGuardianshipAndAdvocacy").show();
                        $("#statusStartGuardianshipAndAdvocacy").hide();
                        $("#statusInprogressGuardianshipAndAdvocacy").hide();
                    }
                    else {
                        $("#statusCompletedGuardianshipAndAdvocacy").hide();
                        $("#statusStartGuardianshipAndAdvocacy").hide();
                        $("#statusInprogressGuardianshipAndAdvocacy").show();
                    }
                
                }
            }
            if (result.AllTabsComprehensiveAssessment[0].AdvancedDirectivesFuturePlanningId!=0) {

                $("#TextBoxAdvancedDirectivesFuturePlanningId").val(result.AllTabsComprehensiveAssessment[0].AdvancedDirectivesFuturePlanningId);

                if (result.AllTabsComprehensiveAssessment[0].Status != null) {
                    var status = result.AllTabsComprehensiveAssessment[0].Status;
                    if (status == "Completed") {
                        $("#statusCompletedAdvancedDirectivesFuturePlanning").show();
                        $("#statusStartAdvancedDirectivesFuturePlanning").hide();
                        $("#statusInprogressAdvancedDirectivesFuturePlanning").hide();
                    }
                    else {
                        $("#statusCompletedAdvancedDirectivesFuturePlanning").hide();
                        $("#statusStartAdvancedDirectivesFuturePlanning").hide();
                        $("#statusInprogressAdvancedDirectivesFuturePlanning").show();
                    }
                
                }
            }
            if (result.AllTabsComprehensiveAssessment[0].IndependentLivingSkillId!=0) {

                $("#TextBoxIndependentLivingSkillId").val(result.AllTabsComprehensiveAssessment[0].IndependentLivingSkillId);

                if (result.AllTabsComprehensiveAssessment[0].Status != null) {
                    var status = result.AllTabsComprehensiveAssessment[0].Status;
                    if (status == "Completed") {
                        $("#statusCompletedIndependentLivingSkills").show();
                        $("#statusStartIndependentLivingSkills").hide();
                        $("#statusInprogressIndependentLivingSkills").hide();
                    }
                    else {
                        $("#statusCompletedIndependentLivingSkills").hide();
                        $("#statusStartIndependentLivingSkills").hide();
                        $("#statusInprogressIndependentLivingSkills").show();
                    }
                
                }
            }
            if (result.AllTabsComprehensiveAssessment[0].SocialServiceNeedId!=0) {

                $("#TextBoxSocialServiceNeedId").val(result.AllTabsComprehensiveAssessment[0].SocialServiceNeedId);

                if (result.AllTabsComprehensiveAssessment[0].Status != null) {
                    var status = result.AllTabsComprehensiveAssessment[0].Status;
                    if (status == "Completed") {
                        $("#statusCompletedSocialServiceNeeds").show();
                        $("#statusStartSocialServiceNeeds").hide();
                        $("#statusInprogressSocialServiceNeeds").hide();
                    }
                    else {
                        $("#statusCompletedSocialServiceNeeds").hide();
                        $("#statusStartSocialServiceNeeds").hide();
                        $("#statusInprogressSocialServiceNeeds").show();
                    }
                
                }
            }
            if (result.AllTabsComprehensiveAssessment[0].MedicalHealthId!=0) {

                $("#TextBoxMedicalHealthId").val(result.AllTabsComprehensiveAssessment[0].MedicalHealthId);

                if (result.AllTabsComprehensiveAssessment[0].Status != null) {
                    var status = result.AllTabsComprehensiveAssessment[0].Status;
                    if (status == "Completed") {
                        $("#statusCompletedMedicalHealth").show();
                        $("#statusStartMedicalHealth").hide();
                        $("#statusInprogressMedicalHealth").hide();
                    }
                    else {
                        $("#statusCompletedMedicalHealth").hide();
                        $("#statusStartMedicalHealth").hide();
                        $("#statusInprogressMedicalHealth").show();
                    }
                
                }
            }
            if (result.AllTabsComprehensiveAssessment[0].HealthPromotionId!=0) {

                $("#TextBoxHealthPromotionId").val(result.AllTabsComprehensiveAssessment[0].HealthPromotionId);

                if (result.AllTabsComprehensiveAssessment[0].Status != null) {
                    var status = result.AllTabsComprehensiveAssessment[0].Status;
                    if (status == "Completed") {
                        $("#statusCompletedHealthPromotion").show();
                        $("#statusStartHealthPromotion").hide();
                        $("#statusInprogressHealthPromotion").hide();
                    }
                    else {
                        $("#statusCompletedHealthPromotion").hide();
                        $("#statusStartHealthPromotion").hide();
                        $("#statusInprogressHealthPromotion").show();
                    }
                
                }
            }
            if (result.AllTabsComprehensiveAssessment[0].BehavioralHealthId!=0) {

                $("#TextBoxBehavioralHealthId").val(result.AllTabsComprehensiveAssessment[0].BehavioralHealthId);

                if (result.AllTabsComprehensiveAssessment[0].Status != null) {
                    var status = result.AllTabsComprehensiveAssessment[0].Status;
                    if (status == "Completed") {
                        $("#statusCompletedBehavioralHealth").show();
                        $("#statusStartBehavioralHealth").hide();
                        $("#statusInprogressBehavioralHealth").hide();
                    }
                    else {
                        $("#statusCompletedBehavioralHealth").hide();
                        $("#statusStartBehavioralHealth").hide();
                        $("#statusInprogressBehavioralHealth").show();
                    }
                
                }
            }
            if (result.AllTabsComprehensiveAssessment[0].ChallengingBehaviorId!=0) {

                $("#TextBoxChallengingBehaviorId").val(result.AllTabsComprehensiveAssessment[0].ChallengingBehaviorId);

                if (result.AllTabsComprehensiveAssessment[0].Status != null) {
                    var status = result.AllTabsComprehensiveAssessment[0].Status;
                    if (status == "Completed") {
                        $("#statusCompletedChallengingBehaviors").show();
                        $("#statusStartChallengingBehaviors").hide();
                        $("#statusInprogressChallengingBehaviors").hide();
                    }
                    else {
                        $("#statusCompletedChallengingBehaviors").hide();
                        $("#statusStartChallengingBehaviors").hide();
                        $("#statusInprogressChallengingBehaviors").show();
                    }
                
                }
            }
            if (result.AllTabsComprehensiveAssessment[0].BehavioralSupportPlanId!=0) {

                $("#TextBoxBehavioralSupportPlanId").val(result.AllTabsComprehensiveAssessment[0].BehavioralSupportPlanId);

                if (result.AllTabsComprehensiveAssessment[0].Status != null) {
                    var status = result.AllTabsComprehensiveAssessment[0].Status;
                    if (status == "Completed") {
                        $("#statusCompletedBehavioralSupportPlan").show();
                        $("#statusStartBehavioralSupportPlan").hide();
                        $("#statusInprogressBehavioralSupportPlan").hide();
                    }
                    else {
                        $("#statusCompletedBehavioralSupportPlan").hide();
                        $("#statusStartBehavioralSupportPlan").hide();
                        $("#statusInprogressBehavioralSupportPlan").show();
                    }
                
                }
            }
            if (result.AllTabsComprehensiveAssessment[0].MedicationId!=0) {

                $("#TextBoxMedicationId").val(result.AllTabsComprehensiveAssessment[0].MedicationId);

                if (result.AllTabsComprehensiveAssessment[0].Status != null) {
                    var status = result.AllTabsComprehensiveAssessment[0].Status;
                    if (status == "Completed") {
                        $("#statusCompletedMedications").show();
                        $("#statusStartMedications").hide();
                        $("#statusInprogressMedications").hide();
                    }
                    else {
                        $("#statusCompletedMedications").hide();
                        $("#statusStartMedications").hide();
                        $("#statusInprogressMedications").show();
                    }
                
                }
            }
            if (result.AllTabsComprehensiveAssessment[0].CommunityParticipationId!=0) {

                $("#TextBoxCommunityParticipationId").val(result.AllTabsComprehensiveAssessment[0].CommunityParticipationId);

                if (result.AllTabsComprehensiveAssessment[0].Status != null) {
                    var status = result.AllTabsComprehensiveAssessment[0].Status;
                    if (status == "Completed") {
                        $("#statusCompletedCommunitySocial").show();
                        $("#statusStartCommunitySocial").hide();
                        $("#statusInprogressCommunitySocial").hide();
                    }
                    else {
                        $("#statusCompletedCommunitySocial").hide();
                        $("#statusStartCommunitySocial").hide();
                        $("#statusInprogressCommunitySocial").show();
                    }
                
                }
            }
        }
    }
    else {
        showErrorMessage(result.Message);
    }
}
//#region Disable buttons enable sections
function DisableSaveButtonChildSection() {
    $(".btnDisable").prop("disabled", true);
    $(".btnNoStatus").prop("disabled", true);
    $("#btnSaveAsNew, #btnPublishVersion").addClass("hidden");
    $("#btnPrintPDf").hide();
    $("#labelAssessmentStatus, #labelDocumentVersion").text("");
    $(".bgProgress").hide();
    $(".bgInprogress").hide();
}
function CreateChildBtnWithPermission(editEvent,deleteEvent) {
    debugger;
    var notificationBtn = "";
    if (!isEmpty(editPermission) && !isEmpty(deletePermission)) {
        if (editPermission == "true" && deletePermission == "false") {
            notificationBtn = '<a href="#" class="editSubRows"  onclick="' + editEvent +'(this);event.preventDefault();">Edit </a>'
                + '<span><a href="#" class="deleteSubRows disable-click" onclick="' + deleteEvent+'(this);event.preventDefault();">Delete</a></span>';
        }
        if (editPermission == "false" && deletePermission == "true") {
            notificationBtn = '<a href="#" class="editSubRows disable-click" onclick="' + editEvent +'(this);event.preventDefault();">Edit </a>'
                + '<span><a href="#" class="deleteSubRows" onclick="' + deleteEvent +'(this); event.preventDefault();">Delete</a></span>';
        }
        if (editPermission == "true" && deletePermission == "true") {
            notificationBtn = '<a href="#" class="editSubRows" onclick="' + editEvent +'(this);event.preventDefault();">Edit </a>'
                + '<span><a href="#" class="deleteSubRows" onclick="' + deleteEvent +'(this); event.preventDefault();">Delete</a></span>';
        }
    }
    return notificationBtn;
}

function AddGuardianshipAndAdvocacy() {
    debugger;
    $("#AddGuardianshipAndAdvocacy").on("click", function () {
        var RadioGuardianshipProof = '';
        if (!$("#AddGuardianshipAndAdvocacy").hasClass("editRow")) {
            if ($("#DropDownWhoHelpMemberMakeDecisionInLife").val() == '' && $("#DropDownHowPersonHelpMemberMakeDecision").val() == '' && $("#TextBoxPersonInvolvementWithMember").val() == '' && $('input[id=SupportsIndividualDecisions]:checked').val() == undefined && $('input[name=RadioGuardianshipProof]:checked').val() == undefined) {
                showErrorMessage(" select atleast one field.");
                return;
            }
            RadioGuardianshipProof = $('input[name=RadioGuardianshipProof]:checked').val();
            if (RadioGuardianshipProof != undefined) {
                RadioGuardianshipProof = $('input[name=RadioGuardianshipProof]:checked').parent('label').text().trim();
            }
            else {
                RadioGuardianshipProof = '';
            }
            if (dataTableFamilyMembersFlg) {
                newRow = $('#GuardianshipAndAdvocacy').DataTable();
                var rowExists = false;
                var valueCol = $('#GuardianshipAndAdvocacy').DataTable().column(1).data();
                var index = valueCol.length;
               
                var rowCount = $('#GuardianshipAndAdvocacy tr').length;
                if (rowCount > 8) {
                    showErrorMessage(" Not allowed more than 8 records");
                    return;

                }
                else {
                    var text = [{
                        "Actions": CreateChildBtnWithPermission("EditGuardianshipAndAdvocacy", "Delete"),

                        "HelpToMakeDecisionLife":  $('#DropDownWhoHelpMemberMakeDecisionInLife option:selected').text(),
                        "HowHelpToMakeDecisions": $('#DropDownHowPersonHelpMemberMakeDecision option:selected').text(),
                        "PersonInvolvementWithMembers":  $("#TextBoxPersonInvolvementWithMember").val(),
                        "SupportsIndividualDecision": $('input[name=SupportsIndividualDecisions]:checked').parent('label').text().trim(),
                        "Guardianship":$('input[name=RadioGuardianshipProof]:checked').parent('label').text().trim(),
                        "HelpToMakeDecisionInLife": $('#DropDownWhoHelpMemberMakeDecisionInLife option:selected').val(),
                        "HowHelpToMakeDecision":$('#DropDownHowPersonHelpMemberMakeDecision option:selected').val(),
                        "PersonInvolvementWithMember": $("#TextBoxPersonInvolvementWithMember").val(),
                        "HelpSignApproveLifePlan": $("#CheckboxHelpSignApproveLifePlan").prop("checked") == true?1:0,
                        "HelpSignApproveMedical":$("#CheckboxHelpSignApproveMedical").prop("checked") == true?1:0,
                        "HelpSignApproveFinancial":$("#CheckboxHelpSignApproveFinancial").prop("checked") == true?1:0,
                        "Other":$("#CheckboxOther").prop("checked") == true?1:0,
                        "GuardianshipProof":$('#RadioGuardianshipProof').val(),

                    }];
                    var stringyfy = JSON.stringify(text);
                    var data = JSON.parse(stringyfy);
                    newRow.rows.add(data).draw(false);
                    showRecordSaved("Family Member added successfully.");

                    clearFamilyMembersFields();
                }

            }
            else {
                var rowExists = false;
                newRow = $('#GuardianshipAndAdvocacy').DataTable();
                var valueCol = $('#GuardianshipAndAdvocacy').DataTable().column(1).data();
                var index = valueCol.length;
               
                var rowCount = $('#GuardianshipAndAdvocacy tr').length;
                if (rowCount > 8) {
                    showErrorMessage("Not allowed more than 8 records");
                    return;
                }
                else {
                    newRow.row.add([
                        CreateChildBtnWithPermission("EditGuardianshipAndAdvocacy", "Delete"),
                       
                        $('#DropDownWhoHelpMemberMakeDecisionInLife option:selected').text(),
                        $('#DropDownHowPersonHelpMemberMakeDecision option:selected').text(),                       
                        $("#TextBoxPersonInvolvementWithMember").val(),
                        $('input[name=SupportsIndividualDecisions]:checked').parent('label').text().trim(),
                        $('input[name=RadioGuardianshipProof]:checked').parent('label').text().trim(),
                        $('#DropDownWhoHelpMemberMakeDecisionInLife option:selected').val(),
                        $('#DropDownHowPersonHelpMemberMakeDecision option:selected').val(),                       
                        $("#TextBoxPersonInvolvementWithMember").val(),
                        $("#CheckboxHelpSignApproveLifePlan").prop("checked") == true?1:0,
                        $("#CheckboxHelpSignApproveMedical").prop("checked") == true?1:0,
                        $("#CheckboxHelpSignApproveFinancial").prop("checked") == true?1:0,
                        $("#CheckboxOther").prop("checked") == true?1:0,                      
                        $('#RadioGuardianshipProof').val()


                    ]).draw(false);
                    showRecordSaved("Added successfully.");

                    clearGuardianshipAndAdvocacy();
                }

            }

        }

    });
}
function clearGuardianshipAndAdvocacy(){

    $(".guardianship").val("");
    $("input[name=RadioGuardianshipProof]").prop('checked', false);
    $('input[id=SupportsIndividualDecisions]:checked').prop('checked', false);

}
function EditGuardianshipAndAdvocacy(object) {
    var table = $('#MembersFamilyConstellation').DataTable();
    currentRowFamilyMembers = $(object).parents("tr");

    //FamilyMembers = table.row(currentRowFamilyMembers).data()[1] == undefined ? table.row(currentRowFamilyMembers).data().Notification : table.row(currentRowFamilyMembers).data()[1];
    var Name = table.row(currentRowFamilyMembers).data()[1] == undefined ? table.row(currentRowFamilyMembers).data().FamilyMemberName : table.row(currentRowFamilyMembers).data()[1];
    var Age = table.row(currentRowFamilyMembers).data()[2] == undefined ? table.row(currentRowFamilyMembers).data().FamilyMemberAge : table.row(currentRowFamilyMembers).data()[2];
    var RelationToClient = table.row(currentRowFamilyMembers).data()[3] == undefined ? table.row(currentRowFamilyMembers).data().FamilyMemberRelation : table.row(currentRowFamilyMembers).data()[3];
    var LivingInHome = table.row(currentRowFamilyMembers).data()[4] == undefined ? table.row(currentRowFamilyMembers).data().FamilyMemberInHome : table.row(currentRowFamilyMembers).data()[4];
    var GeneralInfoFamilyMembersId = table.row(currentRowFamilyMembers).data()[5] == undefined ? table.row(currentRowFamilyMembers).data().GeneralInfoFamilyMembersId : table.row(currentRowFamilyMembers).data()[5];

    $("#TextBoxFamilyMemberName").val(Name);
    $("#TextBoxFamilyMemberAge").val(Age);
    $("#TextBoxFamilyMemberRelation").val(RelationToClient);
    if (LivingInHome == 'Yes') {
        LivingInHome = 1;
    }
    else if (LivingInHome == 'No') {
        LivingInHome = 0;
    }
    else {
        LivingInHome = '';
    }
    LivingInHome == '' ? $("input[name='RadioFamilyMemberInHome']").prop('checked', false) : $("input[name='RadioFamilyMemberInHome'][value=" + LivingInHome + "]").prop('checked', true);
    $("GeneralInfoFamilyMembersId").val(GeneralInfoFamilyMembersId);
    $("#AddMemberOfFamilyConstellation").attr("onclick", "EditExistingRowFamilyMembers();return false;");
    $("#AddMemberOfFamilyConstellation").addClass("editRow");
    $("#AddMemberOfFamilyConstellation").text("Edit");
    return false;
}


