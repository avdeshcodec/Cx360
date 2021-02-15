var token, reportedBy = "", ccoResult, LifePlanId, zipCode, QueryStringLifeplanId, clientId, blankLifePlanId, emptyLifePlan = false,
    approvalStatus,age;
$(document).ready(function () {
    console.log(_token);
    $(window).resize(function () {
        getZoomValues();
        AdjustPaddingResize();
    });
    BindLifePlanDropdowns();
    $(".select2").select2();
    CloseErrorMeeage();
    InitalizeDateControls();
    InitilaizeSectionDataTables();
    AdjustPaddingResize();
    $("#BtnAddMeeting").hide();
    $("#BtnAddIndividualSafe").hide();
    $("#OutcomesSupportStrategies").hide();
    $("#btnModelHCBSWaiver").hide();
    $("#btnModelFundalNaturalCommunity").hide();
    $("#btnModelNotifications").hide();
    $('.section2 .addModal-3').click(function () {
        if ($('html').hasClass('collapse')) {
            $('#collapseTen,#collapseEleven,#collapseSS,#collapsetwelve').removeClass('show');
        } else {
            $('.collapse').removeClass('show');
        }

    });

    $('.section3 .addModal-3').click(function () {
        if ($('html').hasClass('collapse')) {
            $('#collapseNine,#collapseEleven,#collapseSS,#collapsetwelve').removeClass('show');
        } else {
            $('.collapse').removeClass('show');
        }

    });
    EpinSignatureValidation();
    $('.section4 .addModal-3').click(function () {
        if ($('html').hasClass('collapse')) {
            $('#collapseNine,#collapseTen,#collapseSS,#collapsetwelve').removeClass('show');
        } else {
            $('.collapse').removeClass('show');
        }
    });

    $('.section5 .addModal-3').click(function () {
        if ($('html').hasClass('collapse')) {
            $('#collapseNine,#collapseTen,#collapseEleven,#collapsetwelve').removeClass('show');
        } else {
            $('.collapse').removeClass('show');
        }
    });
    $('.section6 .addModal-3').click(function () {
        if ($('html').hasClass('collapse')) {
            $('#collapseNine,#collapseTen,#collapseEleven,#collapseSS').removeClass('show');
        } else {
            $('.collapse').removeClass('show');
        }
    });
    $("input[name='select_all']").click(function () {

        if ($("input[name='select_all']:checked").length > 0) {
            $("#tblStrategiesOutcomesSupportStrategies input[name='RadioSuggestedSupportStrategie']").prop('checked', true);
        }
        else {
            $("#tblStrategiesOutcomesSupportStrategies input[name='RadioSuggestedSupportStrategie']").prop('checked', false);
        }
    });
    $("input[name='select_all_meeting']").click(function () {

        if ($("input[name='select_all_meeting']:checked").length > 0) {
            $("#tblMeetingHistoryExported input[name='RadioMeetingHistory']").prop('checked', true);
        }
        else {
            $("#tblMeetingHistoryExported input[name='RadioMeetingHistory']").prop('checked', false);
        }
    });
    $("#imuMeetingHistory .clearBtn").on('click', function () {
        clearMeetingHistorytext();
    });
    $("#lfuFromOutcomesStrategies .clearBtn").on('click', function () {
        cleartextOutcomesStrategies();
    });
    $("#imuIndividualSafe .clearBtn").on('click', function () {
        clearIndividualSavetext();
    });
    $("#lfuFromFundalNaturalCommunityResources .clearBtn").on('click', function () {
        cleartextFundalNaturalCommunityResources();
    });
    $("#lfuFromHCBSWaiver .clearBtn").on('click', function () {
        cleartextHCBSWaiver();
    });
    $("#lfuNotifications .clearBtn").on('click', function () {
        cleartextLifePlanNotifications();
    });
    $(".submitReviewModal .clearBtn").on('click', function () {
        clearsubmitReviewModal();
    });

    $('#TextBoxFundalResourcesZip').on("change", function () {
        if ($(this).val() != "") {
            $.ajax({
                type: "GET",
                data: { "ZipCode": $(this).val() },
                url: "https://staging-api.cx360.net/api/Incident/GetZipDetails",
                headers: {
                    'Token': token,
                },
                success: function (response) {
                    if (response.length > 0) {
                        $("#TextBoxFundalCity").val(response[0].City);
                        $("#TextBoxFundalState").val(response[0].State);
                    }
                    else {
                        $("#TextBoxFundalCity").val("");
                        $("#TextBoxFundalState").val("");
                    }
                },
                error: function (xhr) { HandleAPIError(xhr) }
            });
        }
    });

    $("#TextBoxEffectiveTo").focus(function () {
        if ($("#TextBoxEffectiveFrom").val() == null || $("#TextBoxEffectiveFrom").val() == "") {
            $("#TextBoxEffectiveTo").val("");
            $('#TextBoxEffectiveFrom').focus();
            showErrorMessage("Please Select Effective From Date ");
        }
    });

    $("#TextBoxEffectiveToDate").focus(function () {
        if ($("#TextBoxEffectiveFromDate").val() == null || $("#TextBoxEffectiveFromDate").val() == "") {
            $("#TextBoxEffectiveToDate").val("");
            $('#TextBoxEffectiveFromDate').focus();
            showErrorMessage("Please Select Effective From Date ");
        }
    });

    QueryStringLifeplanId = GetParameterValues('LifePlanId');
    var DocumentVersionId = GetParameterValues('DocumentVersionId');
    if (QueryStringLifeplanId > 0 && DocumentVersionId > 0) {
        ManageExistingLifePlan(QueryStringLifeplanId, DocumentVersionId);
    }
    else {
        $(".loader").hide();
        $(".hide-lifeplan-btn").addClass("hidden");
        $("#btnPrintPDf").hide();
        $("#labelLifePlanStatus, #labelDocumentVersion").text("");
        $("#btnAssessmentNarrativesummary").hide();
    }


     


});
function ManageExistingLifePlan(QueryStringLifeplanId, DocumentVersionId) {

    $.ajax({
        type: "POST",
        data: { TabName: "LifePlanMasterPage", LifePlanId: QueryStringLifeplanId, Json: "", ReportedBy: 1, Mode: "selectById" },
        url: GetAPIEndPoints("HANDLELIFEPLANDETAIL"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (response) {
            if (response.Success == true) {
                setTimeout(function () {
                    $(".loader").fadeOut("slow");
                    FillLifePlanPage(response);
                }, 9000

                );
               
            }
            else {
                showErrorMessage(result.Message);
            }
        },
        error: function (xhr) { HandleAPIError(xhr) }
    });

}


function BindLifePlanDropdowns() {
    token = _token;
    reportedBy = _userId;

    BindDropDowns();
    BindCCOControls();
    GetScreenRolePermissions();
   // BindUserDefinedCodes("#DropDownCqlPomsGoal", "ValuedOutcomes_CQLPOMSGoal",_age);
   
    BindUserDefinedCodes("#DropDownServicesType", "LifePlan_ServiceType");
    BindUserDefinedCodes("#DropDownFrequency", "LifePlan_Frequency");
    BindUserDefinedCodes("#DropDownQuantity", "LifePlan_Quantity");
    BindUserDefinedCodes("#DropDownTimeFrame", "LifePlan_TimeFrame");
    BindUserDefinedCodes("#DropDownServicesTypeIPOP", "LifePlan_ServiceType");
    BindUserDefinedCodes("#DropDownFrequencyIPOP", "LifePlan_Frequency");
    BindUserDefinedCodes("#DropDownQuantityIPOP", "LifePlan_Quantity");
    BindUserDefinedCodes("#DropDownTimeFrameIPOP", "LifePlan_TimeFrame");
    BindUserDefinedCodes("#DropDownTypeOfMeeting", "NoteType");

    BindUserDefinedCodes("#DropDownNotificationReason", "UDO_DocumentTracker_NotificationReason");
    BindUserDefinedCodes("#DropDownNotificationAccptAckwStatus", "UDO_DocumentTracker_FollowUPAction");
    BindUserDefinedCodes("#DropDownReviewStatus", "ISPStatus");
    BindUserDefinedCodes("#DropDownNotificationType", "ContactMethod");
    BindUserDefinedCodes("#DropDownSubmitStatus", "SubmissionStatus");
    BindAllStaff("#DropDownSubmittedTo");
}
     
function GetScreenRolePermissions() {
    $.ajax({
        type: "GET",
        url: 'https://staging-api.cx360.net/api/Common/UserFormAccess',
        data: { 'FormName': "Client Profile" },
        headers: {
            'TOKEN': token
        },
        success: function (result) {
            BindUserRolePermissions(result);

        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}
function BindUserRolePermissions(result) {
    var array = [];
    if (result != null) {
        $.each(result, function (key, value) {
            array.push(value.Action);
        });
    }
    if (QueryStringLifeplanId == undefined || emptyLifePlan==true) {
        AddingNewRecord(array);
    }
    else {
        if (array.includes("View")) {
            ViewPermissionLifePlan();
        }
        else {
            window.location.href = 'PermissionDenied.html';
        }
        if (array.includes("Edit")) {
            EditViewPermissionLifePlan();
        }
        if (array.includes("Print")) {
            ViewPdfPermissionLifePlan();
        }
    }

}
function AddingNewRecord(array) {
    if (!array.includes("View")) {
        window.location.href = 'PermissionDenied.html';
    }

    else if (array.includes("Add")) {
        $("#btnLifePlan").text("Ok");
        $("#btnLifePlan").show();
    }
    else {
        $(".editRecord").hide();
        $(".addModal-2").addClass('hidden');
        $(".addModal-3").addClass('hidden');
    }
    if (array.includes("Print")) {
    }
}
function FillLifePlanPage(response) {
    debugger;
    if (response.LifPlanDetailsData != null || response.LifPlanDetailsData != undefined) {
        var DBO = (response.LifPlanDetailsData[0].DateOfBirth);
        DBO = DBO.slice(0, 10).split('-');
        DBO = DBO[1] + '/' + DBO[2] + '/' + DBO[0];
        _age=DBO;
        var EnrollmentDate = (response.LifPlanDetailsData[0].EnrollmentDate);
        EnrollmentDate = EnrollmentDate.slice(0, 10).split('-');
        EnrollmentDate = EnrollmentDate[1] + '/' + EnrollmentDate[2] + '/' + EnrollmentDate[0];

        var EffectiveFromDate = (response.LifPlanDetailsData[0].EffectiveFromDate);
        EffectiveFromDate = EffectiveFromDate.slice(0, 10).split('-');
        EffectiveFromDate = EffectiveFromDate[1] + '/' + EffectiveFromDate[2] + '/' + EffectiveFromDate[0];

        var EffectiveToDate = (response.LifPlanDetailsData[0].EffectiveToDate);
        EffectiveToDate = EffectiveToDate.slice(0, 10).split('-');
        EffectiveToDate = EffectiveToDate[1] + '/' + EffectiveToDate[2] + '/' + EffectiveToDate[0];

        //$('.lifePlanId').val(MemberId);
        
        $('#TextBoxLifePlanId').val(response.LifPlanDetailsData[0].LifePlanId);
        $('#TextBoxDocumentVersionId').val(response.LifPlanDetailsData[0].DocumentVersionId);
        $('#labelLifePlanStatus').text(response.LifPlanDetailsData[0].DocumentStatus);
        $('#labelDocumentVersion').text(Number(response.LifPlanDetailsData[0].DocumentVersion).toFixed(1));
        $('#DropDownClientId').select2('val', [response.LifPlanDetailsData[0].ClientId]);
         var clentName = $('#DropDownClientId ').find(':selected').text();
        $('#TextBoxRepresentativeMemberName').val(clentName);    
        $('#TextBoxDateOfBirth').val(DBO);
        $('#TextBoxMemberAddress1').val(response.LifPlanDetailsData[0].MemberAddress1);
        $('#TextBoxMemberAddress2').val(response.LifPlanDetailsData[0].MemberAddress2);
        $('#TextBoxCity').val(response.LifPlanDetailsData[0].AddressCCO);
        $('#TextBoxPhone').val(response.LifPlanDetailsData[0].Phone);

        $('#TextBoxMedicaid').val(response.LifPlanDetailsData[0].Medicaid);
        $('#TextBoxMedicare').val(response.LifPlanDetailsData[0].Medicare);
        $('#TextBoxEnrollmentDate').val(EnrollmentDate);//.val(response.LifPlanDetailsData[0].EnrollmentDate);
        $('#TextBoxWillowbrookMember').val(response.LifPlanDetailsData[0].WillowbrookMember);
        $('#TextBoxEffectiveFromDate').val(EffectiveFromDate);//.val(response.LifPlanDetailsData[0].EffectiveFromDate);
        $('#TextBoxEffectiveToDate').val(EffectiveToDate);//.val(response.LifPlanDetailsData[0].EffectiveToDate);
        $('#TextBoxAddressCCO').val(response.LifPlanDetailsData[0].AddressCCO);
        $('#TextBoxPhoneCCO').val(response.LifPlanDetailsData[0].PhoneCCO);
        $('#TextBoxFaxCCO').val(response.LifPlanDetailsData[0].Fax);
        $('#TextBoxProviderID').val(response.LifPlanDetailsData[0].ProviderID);
        $('#TextMBoxLifePlanId').val(response.LifPlanDetailsData[0].LifePlanId);
        $('#TextBoxOutcomesStrategiesLifePlanId').val(response.LifPlanDetailsData[0].LifePlanId);
        $('#TextIBoxLifePlanId').val(response.LifPlanDetailsData[0].LifePlanId);
        $('#TextBoxHCBSLifePlanId').val(response.LifPlanDetailsData[0].LifePlanId);
        $('#TextBoxFundalResourcesLifePlanId').val(response.LifPlanDetailsData[0].LifePlanId);
        $('#TextAreaLifePlanId').val(response.LifPlanDetailsData[0].LifePlanId);
        $("#TextBoxLifePlanId").val(response.LifPlanDetailsData[0].LifePlanId);

        clientId = response.LifPlanDetailsData[0].ClientId;

        BindGetList("#DropDownProviderLocation", "Network Provider List", clientId);
        BindGetList("#IndividualProviderLocation", "Network Provider List", clientId);
        BindGetList("#DropDownFacilityName", "Network Provider List", clientId);
        BindGetList("#DropDownCode", "Services", clientId);
        BindGetList("#DropDownNotificationProvider", "Network Provider List", clientId);
        //CheckLifePlanPageDetailBeforeAdd();
        GetAssessmentNarrativeSummaryTabDetails();
        GetMeetingHistoryTabDetails();
        GetIndividualSafeTabDetails();
        GetOutcomesStrategiesTabDetails();
        GetHCBSWaiverTabDetails();
        GetFundalNaturalCommunityResourcesTabDetails();
        GetLifePlanTabDetails();
   
        ShowHideButtons(response.LifPlanDetailsData[0].DocumentStatus, response.LifPlanDetailsData[0].LatestVersion, response.LifPlanDetailsData[0].Status);
            
        GetSuggestedOutcomesStrategiesTabDetails(clientId);
        GetDefaultMeetingHistoryDetails(clientId);
        BindUserDefinedCodes("#DropDownCqlPomsGoal", "ValuedOutcomes_CQLPOMSGoal",_age);
    }
    else {
        $("#btnSaveAsNew, #btnPublishVersion").addClass("hidden");
        $("#btnPrintPDf").hide();
        $("#labelLifePlanStatus, #labelDocumentVersion").text("");
        $("#DropDownClientId").prop("disabled", false);
        $("#btnLifePlan").show();
        $("#btnLifePlan").text("Ok");
        $("#imuLifePlan .LifePlanEnable").prop("disabled", false);
        $("#btnAssessmentNarrativesummary").hide();
        emptyLifePlan = true;
    }
}

//#region Permissions methods
function ShowHideButtons(status, version,submitStatus) {
    if (status == "Published" && version == true) {
        $("#btnSaveAsNew").removeClass("hidden");
        $("#btnPrintPDf").show();
        $("#btnPublishVersion").addClass("hidden");
        $(".addModal-2").addClass('hidden');
        $(".addModal-3").addClass('hidden');
       

        $(".editRecord").hide();
    
        $("#btnModelNotifications").show();
        $("#btnSubmitApproval").hide();
        $("#btnReviewApproval").hide();
        $(".section6 .greencolor").prop("disabled", false);
        $(".section6 .redcolor").prop("disabled", false);
        setTimeout(function () {
            $(".greencolor").prop("disabled", true);
            $(".redcolor").prop("disabled", true);
            $(".section6 .greencolor").prop("disabled", false);
            $(".section6 .redcolor").prop("disabled", false);
        }, 2000);
    }
    else if (status == "Published" && version == false) {
        $("#btnSaveAsNew,  #btnPublishVersion").addClass("hidden");
        $("#btnPrintPDf").show();
        $(".addModal-2").addClass('hidden');
        $(".addModal-3").addClass('hidden');
        $(".editRecord").hide();
       
        $("#btnModelNotifications").show();
        $("#btnSubmitApproval").hide();
        $("#btnReviewApproval").hide();
        setTimeout(function () {
            $(".greencolor").prop("disabled", true);
            $(".redcolor").prop("disabled", true);
            $(".section6 .greencolor").prop("disabled", false);
            $(".section6 .redcolor").prop("disabled", false);
        }, 2000);
    }
    else if (status == "Draft") {
        $("#btnSaveAsNew").addClass("hidden");
        $("#btnPrintPDf").show();
        $("#btnPublishVersion").removeClass("hidden");
        $(".editRecord").text("Edit");
        $(".editRecord").show();
        $(".addModal-2").show();
        $(".addModal-3").show();
       
        $('#lfuFromAssessmentNarrativeSummary .LifePlanEnable').prop("disabled", true);
        $("#btnSubmitApproval").show();
        ShowHideSubmitReviewButton(submitStatus);
        setTimeout(function () {
            $(".greencolor").prop("disabled", false);
            $(".redcolor").prop("disabled", false);
        }, 2000);
    }
}
function ViewPermissionLifePlan() {
    $('#imuLifePlan .LifePlan-control').attr("disabled", true);
    $(".editRecord").hide();
    $(".printDoc").hide();
    $(".addModal-2").addClass('hidden');
    $(".addModal-3").addClass('hidden');
    $(".greenColor").attr("disabled", true);
    $(".redColor").attr("disabled", true);
    $('#lfuFromAssessmentNarrativeSummary .LifePlanEnable').attr("disabled", true);
}
function EditViewPermissionLifePlan() {
    $('#imuLifePlan .LifePlan-control').attr("disabled", true);
    $(".editRecord").text("Edit");
    $(".printDoc").hide();
    $(".addModal-2").removeClass('hidden');
    $(".addModal-3").removeClass('hidden');
    $(".greenColor").attr("disabled", false);
    $(".redColor").attr("disabled", false);
    $('#lfuFromAssessmentNarrativeSummary .LifePlanEnable').attr("disabled", true);
}
function ViewPdfPermissionLifePlan() {
    $('#imuLifePlan .LifePlan-control').attr("disabled", true);
    $(".editRecord").hide();
    $(".printDoc").show();
    $(".addModal-2").addClass('hidden');
    $(".addModal-3").addClass('hidden');
    $(".greenColor").attr("disabled", true);
    $(".redColor").attr("disabled", true);
    $('#lfuFromAssessmentNarrativeSummary .LifePlanEnable').attr("disabled", true);
}
function AllPermissionLifePlan() {
    $('#imuLifePlan .LifePlan-control').attr("disabled", true);
    $(".editRecord").text("Edit");
    $(".printDoc").show;
    $(".addModal-2").removeClass('hidden');
    $(".addModal-3").removeClass('hidden');
    $(".greenColor").attr("disabled", false);
    $(".redColor").attr("disabled", false);
    $('#lfuFromAssessmentNarrativeSummary .LifePlanEnable').attr("disabled", true);
}
//#endregion


function InitalizeDateControls() {
    InitCalendar($(".date"), "date controls");

    $('.time').timepicker(getTimepickerOptions());
    $('.time').on("timeFormatError", function (e) { timepickerFormatError($(this).attr('id')); });
}
function InitilaizeSectionDataTables() {
    var table2 = $('#tblIndividualSafety').dataTable({ "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]], searching: true, paging: true, info: true });
    var table4 = $('#tblOutcomesSupportStrategies').DataTable({ "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]], searching: true, paging: true, info: true });


    var table3 = $('#tblMeetingHistory').dataTable({ "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]], searching: true, paging: true, info: true });
    var table1 = $('#tblHCBSWaiver').DataTable({ "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]], pageLength: 0, searching: true, paging: true, info: true });
    var table = $('#tblFundalNaturalCommunityResources').DataTable({ "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]], pageLength: 0, searching: true, paging: true, info: true });
    var table5 = $('#tblLifeplanNotifications').DataTable({ "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]], searching: true, paging: true, info: true });
    var tableMemberRepresentative = $('#tblMemberRepresentativeApproval').DataTable({ "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]], pageLength: 0, searching: true, paging: true, info: true });
    var table = $('#tblMember').DataTable({ "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]], pageLength: 0, searching: true, paging: true, info: true });
    jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');

}
function BindDropDowns() {

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

//#region Lifeplan
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


        var EnrollmentDate = (res[0]["Enrollment Date"]);
        if (EnrollmentDate != null) {
            EnrollmentDate = EnrollmentDate.slice(0, 10).split('-');
            EnrollmentDate = EnrollmentDate[1] + '/' + EnrollmentDate[2] + '/' + EnrollmentDate[0];
        }

        //var phone = (res[0].Phone);
        //phone = phone, formatted = phone.substr(0, 3) + '-' + phone.substr(3, 3) + '-' + phone.substr(6, 4)
        _age=DBO
        $("#TextBoxDateOfBirth").val(DBO)//.val(res[0].BirthDate)
        $("#TextBoxMemberAddress1").val(res[0].Address1)
        $("#TextBoxMemberAddress2").val(res[0].Address2)
        $("#TextBoxCity").val(res[0].City)
        $("#TextBoxState").val(res[0].State)
        $("#TextBoxZipCode").val(res[0].ZipCode)
        //$("#TextBoxPhone").val(res[0].Phone)
        $('#TextBoxPhone').val(formatPhoneNumberClient(res[0].Phone));
        $("#TextBoxMedicaid").val(res[0]["Medicaid Number"])
        $("#TextBoxMedicare").val(res[0]["Medicare Number"])
        $("#TextBoxEnrollmentDate").val(EnrollmentDate)//.val(res[0]["Enrollment Date"])
        $("#TextBoxWillowbrookMember").val(res[0].WillowBrook)

        BindGetList("#DropDownProviderLocation", "Network Provider List", clientId);
        BindGetList("#IndividualProviderLocation", "Network Provider List", clientId);
        BindGetList("#DropDownFacilityName", "Network Provider List", clientId);
        BindGetList("#DropDownCode", "Services", clientId);
        BindGetList("#DropDownNotificationProvider", "Network Provider List", clientId);

        GetSuggestedOutcomesStrategiesTabDetails(clientId);
        GetDefaultMeetingHistoryDetails(clientId);
        BindUserDefinedCodes("#DropDownCqlPomsGoal", "ValuedOutcomes_CQLPOMSGoal",_age);
        
    }

}
function FillClientDetails(object) {
    var selectedValue = $(object).val();
    var jsonObject = $("#DropDownClientId").attr("josn");
    var parse = jQuery.parseJSON(jsonObject);
    var res = $.grep(parse, function (IndividualNmae) {
        return IndividualNmae.ClientID == selectedValue;
    });
    var DBO = (res[0].BirthDate);
    if (DBO != null) {

        DBO = DBO.slice(0, 10).split('-');
        DBO = DBO[1] + '/' + DBO[2] + '/' + DBO[0];
    }
    var EnrollmentDate = (res[0]["Enrollment Date"]);
    if (EnrollmentDate != null) {
        EnrollmentDate = EnrollmentDate.slice(0, 10).split('-');
        EnrollmentDate = EnrollmentDate[1] + '/' + EnrollmentDate[2] + '/' + EnrollmentDate[0];
    }



    //var phone = (res[0].Phone);
    //phone = phone, formatted = phone.substr(0, 3) + '-' + phone.substr(3, 3) + '-' + phone.substr(6, 4)
    $("#TextBoxDateOfBirth").val(DBO)//.val(res[0].BirthDate)
    $("#TextBoxMemberAddress1").val(res[0].Address1)
    $("#TextBoxMemberAddress2").val(res[0].Address2)
    $("#TextBoxCity").val(res[0].City)
    $("#TextBoxState").val(res[0].State)
    $("#TextBoxZipCode").val(res[0].ZipCode)
    //$("#TextBoxPhone").val(res[0].Phone)
    $('#TextBoxPhone').val(formatPhoneNumberClient(res[0].Phone));
    $("#TextBoxMedicaid").val(res[0].MedicaidNumber)
    $("#TextBoxMedicare").val(res[0].MedicareNumber)
    $("#TextBoxEnrollmentDate").val(EnrollmentDate)//.val(res[0]["Enrollment Date"])
    $("#TextBoxWillowbrookMember").val(res[0].WillowBrook)

}
function BindCCOControls() {
    debugger;
    //get all clients
    $.ajax({
        type: "GET",
        url: 'https://staging-api.cx360.net/api/Incident/GetCompanyDetails',
        headers: {
            'TOKEN': token,
        },
        success: function (result) {
            $("#TextBoxAddressCCO").val(result[0].Address1);
            $("#TextBoxAddress2CCO").val(result[0].Address2);
            $("#DropDownCityCCO").val(result[0].City)
            $("#DropDownStateCCO").val(result[0].State)
            //$("#DropDownCityCCO").append($("<option></option>").val(result[0].City).html(result[0].City));
            //$("#DropDownStateCCO").append($("<option></option>").val(result[0].State).html(result[0].State));;
            $("#TextBoxZipCodeCCO").val(result[0].Zipcode);
            $("#TextBoxPhoneCCO").val(result[0].Phone);
            $("#TextBoxFaxCCO").val(result[0].Fax);

        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}
function InsertModifyLifePlan() {
    if ($("#btnLifePlan").text() == "Edit") {
        $("#btnLifePlan").text("Ok");
        $('#imuLifePlan .LifePlanEnable').attr("disabled", false);
        //$('#imuLifePlan  input[type=radio]').prop("disabled", false);
        $("#btnLifePlan").text("Ok");
        return;
    }
    if (!validateLifePlanTab()) return;
    zipCode = $("#TextBoxZipCode").val();
    blankLifePlanId = $("#TextBoxLifePlanId").val();
    var json = [],
        item = {},
        tag;
    $('#imuLifePlan .LifePlan-control').each(function () {
        tag = $(this).attr('name').replace("TextBox", "").replace("Checkbox", "").replace("DropDown", "").replace("Radio", "").replace("TextBox1", "");
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
            else {
                item[tag] = jsonWrapperWithTimePicker(tag, this);
            }
        }
    });
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "LifePlan", Json: JSON.stringify(json), ReportedBy: reportedBy },
        url: GetAPIEndPoints("INSERTMODIFYLIFEPLANDETAIL"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                LifePlanTabSaved(result);


            }
            else {
                showErrorMessage(result.Message);
            }

        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}
function validateLifePlanTab() {
    var checked = null;
    var startDate = $("#TextBoxEffectiveFromDate").val();
    var endDate = $("#TextBoxEffectiveToDate").val();

    if ((Date.parse(startDate) >= Date.parse(endDate))) {
        //alert("End date should be greater than Start date");
        showErrorMessage("Effective To Date should be greater than Effective From Date");
        $("#TextBoxEffectiveToDate").val("");
        $('#TextBoxEffectiveToDate').focus();
        checked = false;
        return checked;
    }

    $("#imuLifePlan .req_feild").each(function () {
        if ($(this).attr("type") == "radio") {
            var radio = $(this).attr("name");
            if ($('input[name=' + radio + ']:checked').length == 0) {
                $(this).parent().parent().parent().next().children().removeClass("hidden");
                $(this).focus();
                checked = false;
                return checked;
            }
        }
        else if ($(this).val() == "" || $(this).val() == "-1") {
            $(this).siblings("span.errorMessage").removeClass("hidden");
            $(this).focus();
            checked = false;
            return checked;
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
        if ($(this).attr("type") == "radio" && $(this).hasClass("req_feild")) {
            var radio = $(this).attr("name");
            $('input[name=' + radio + ']').change(function () {
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
function LifePlanTabSaved(result) {
    if (result.Success == true && result.IsException == false) {
        if (result.AllTab[0].ValidatedRecord == false) {
            showErrorMessage("Life plan already exists in Draft for client");
            return;
        }
        else {
            if (result.AllTab[0].LifePlanId > -1) {
                $("#TextBoxLifePlanId").val(result.AllTab[0].LifePlanId);
                showRecordSaved('Record Saved');
                $("#TextBoxDocumentVersionId").val(result.AllTab[0].DocumentVersionId);
                if (result.AllTab[0].DocumentVersion != "") {
                    $("#labelLifePlanStatus").text(result.AllTab[0].DocumentStatus);
                    $("#labelDocumentVersion").text(result.AllTab[0].DocumentVersion);
                }
                CheckLifePlanPageDetailBeforeAdd();
                ShowHideSubmitReviewButton(result.AllTab[0].Status);
                $("#btnSaveAsNew").addClass("hidden");
                $("#btnPublishVersion").removeClass("hidden");
                $("#btnPrintPDf").show();
                $('#imuLifePlan .LifePlan-control').attr("disabled", true);
                if ($("#btnLifePlan").text() == "Ok") {
                    $("#btnLifePlan").text("Edit");
                }
                $("#TextMBoxLifePlanId").val(result.AllTab[0].LifePlanId);
                $("#TextIBoxLifePlanId").val(result.AllTab[0].LifePlanId);
                $("#TextBoxHCBSLifePlanId").val(result.AllTab[0].LifePlanId);
                $("#TextBoxFundalResourcesLifePlanId").val(result.AllTab[0].LifePlanId);
                $("#TextAreaLifePlanId").val(result.AllTab[0].LifePlanId);
                $("#TextBoxOutcomesStrategiesLifePlanId").val(result.AllTab[0].LifePlanId);
                changeLifePlanURL(result.AllTab[0].LifePlanId, result.AllTab[0].DocumentVersionId);

            }
           


        }


    }
}
function CheckLifePlanPageDetailBeforeAdd() {
    if ($("#TextBoxLifePlanId").val() != null) {
        $("#BtnAddMeeting").show();
        $("#BtnAddIndividualSafe").show();
        $("#OutcomesSupportStrategies").show();
        $("#btnModelHCBSWaiver").show();
        $("#btnModelFundalNaturalCommunity").show();
        $("#btnAssessmentNarrativesummary").show();
        $("#btnModelNotifications").show();
    }
}
function changeLifePlanURL(lifePlanId, documentVersionId) {
    if (blankLifePlanId < 1) {
        var currentURL = window.location.href.split('?')[0];
        var newURL = currentURL + "?LifePlanId=" + lifePlanId + "&DocumentVersionId=" + documentVersionId;
        history.pushState(null, 'Life Plan Template', newURL);
    }
}
function ShowHideSubmitReviewButton(status) {
    if (isEmpty(status) || status == "Correction Requested") {
        $("#btnSubmitApproval").show();
        $("#btnReviewApproval").hide();
    }
    else if (status == "Submitted") {
        $("#btnSubmitApproval").hide();
        $("#btnReviewApproval").show();
    }
    else {
        $("#btnSubmitApproval").hide();
        $("#btnReviewApproval").hide();
    }
}
//#endregion

//#region MeetingHistory
function InsertModifyMeetingHistory() {
    if (!validateMeetingHistoryTab()) return;
    var json = [],
        item = {},
        tag;
    $('#imuMeetingHistory .MH-control').each(function () {
        tag = $(this).attr('name').replace("TextMBox", "").replace("TextBox", "").replace("Checkbox", "").replace("DropDown", "").replace("Radio", "");
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
            else {
                item[tag] = jsonWrapperWithTimePicker(tag, this);
            }

        }
    });
    item["DocumentVersionId"] = $("#TextBoxDocumentVersionId").val();
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "MeetingHistory", Json: JSON.stringify(json), ReportedBy: reportedBy },
        url: GetAPIEndPoints("INSERTMODIFYLIFEPLANDETAIL"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                $("#TextBoxMeetingId").val("");
                if ($("#TextBoxMeetingId").val() == null || $("#TextBoxMeetingId").val() == "") {

                    showRecordSaved("Record Saved.");

                }
                else {
                    showRecordSaved("Record Updated.");

                }

                GetMeetingHistoryTabDetails();
                clearMeetingHistorytext();
                $("#meetingHistoryModal").modal("hide");
            }
            else {
                showErrorMessage(result.Message);
            }

        },

        error: function (xhr) { HandleAPIError(xhr) }
    });
}
function InsertModifyMeetingHistoryExported() {
    if (!valiateMeetingHistoryExported()) return;
    var JsonMeetingExported = [];

    if ($('#tblMeetingHistoryExported input[name="RadioMeetingHistory"]:checked').length >= 1) {
        var checkedRowLength = $('#tblMeetingHistoryExported input[type="checkbox"]:checked').length;
        $('#tblMeetingHistoryExported input[name="RadioMeetingHistory"]:checked').each(function () {
            var meetingHistory = {};

            meetingHistory["TypeOfMeeting"] = $("#tblMeetingHistoryExported tbody tr:eq(" + $(this).val() + ") td:eq(7)").text();
            meetingHistory["PlanerReviewDate"] = $("#tblMeetingHistoryExported tbody tr:eq(" + $(this).val() + ") td:eq(2)").text();
            meetingHistory["MeetingReason"] = $("#tblMeetingHistoryExported tbody tr:eq(" + $(this).val() + ") td:eq(3)").text();
            meetingHistory["MemberAttendance"] = $("#tblMeetingHistoryExported tbody tr:eq(" + $(this).val() + ") td:eq(4)").text();
            meetingHistory["InformationPresented"] = $("#tblMeetingHistoryExported tbody tr:eq(" + $(this).val() + ") td:eq(5)").text();
            meetingHistory["InformationDiscussed"] = $("#tblMeetingHistoryExported tbody tr:eq(" + $(this).val() + ") td:eq(6)").text();
            meetingHistory["DocumentVersionId"] = $("#TextBoxDocumentVersionId").val();
            meetingHistory["LifePlanId"] = $("#TextBoxLifePlanId").val();
            meetingHistory["NoteID"] = $("#TextBoxNoteID").val();
            JsonMeetingExported.push(meetingHistory);
        });

    }
    $.ajax({
        type: "POST",
        data: { TabName: "MeetingHistory", Json: JSON.stringify(JsonMeetingExported), ReportedBy: reportedBy },
        url: GetAPIEndPoints("INSERTMODIFYLIFEPLANDETAIL"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                if ($("#TextBoxMeetingId").val() == null || $("#TextBoxMeetingId").val() == "") {

                    showRecordSaved("Record Saved.");

                }
                else {
                    showRecordSaved("Record Updated.");

                }
                GetMeetingHistoryTabDetails();
                clearMeetingHistorytext();
                $("#exampleModal").modal("hide");
            }
            else {
                showErrorMessage(result.Message);
            }


        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}
function validateMeetingHistoryTab() {
    if (($("#DropDownTypeOfMeeting").val() == "" && $("#TextBoxPlanerReviewDate").val() == "" && $("#TextBoxMeetingReason").val() == "" && $("#TextBoxMemberAttendance").val() == "" && $("#TextBoxInformationPresented").val() == "" && $("#TextBoxInformationDiscussed").val() == "") ) {
        showErrorMessage("Please enter atleast single meeting history value.")
        return false;
    }
    return true;
}
function valiateMeetingHistoryExported() {
    if ($('#tblMeetingHistoryExported input[name="RadioMeetingHistory"]:checked').length < 1) {
        showErrorMessage("Please select atleast on checkbox.");
        return false;
    }
    return true;
}
function GetMeetingHistoryTabDetails() {
    var json = [],
        item = {},
        tag = "LifePlanId";
    if ($("#TextMBoxLifePlanId").val() != "") {
        item[tag] = $("#TextMBoxLifePlanId").val();
        json.push(item);
        $.ajax({
            type: "POST",
            data: { TabName: "GetMeetingHistory", LifePlanId: $("#TextMBoxLifePlanId").val(), Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "select" },
            dataType: 'json',
            url: GetAPIEndPoints("HANDLEMEETINGTAIL"),
            headers: { 'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5" },
            success: function (result) {

                if (result.Success == true) {
                    var jsonStringyfy = JSON.stringify(result.MeetingHistorySummaryTab) == "null" ? "{}" : JSON.stringify(result.MeetingHistorySummaryTab);
                    $('#tblMeetingHistory').DataTable({
                        'destroy': true,
                        "paging": true,
                        "searching": true,
                        "autoWidth": false,
                        "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
                        "aaData": JSON.parse(jsonStringyfy),
                        "columns": [{ "data": "NoteTypeText" },{ "data": "EventDate" }, { "data": "Subject" }, { "data": "MeetingReason" }, { "data": "Actions" }]
                    });
                    jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');
                }
                else {
                    showErrorMessage(result.Message);
                }
            },
            error: function (xhr) { HandleAPIError(xhr) }
        });
    }

}
function EditMeetingHistoryTabDetails(e, id) {
    var json = [],
        item = {},
        tag = "Id";

    item[tag] = id;
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "GetMeetingHistory", LifePlanId: "", Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "selectById" },
        url: GetAPIEndPoints("HANDLEMEETINGTAIL"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                if (result.MeetingHistorySummaryTab.length == 1) {
                    // InitalizeDateControls();
                    $("#TextBoxPlanerReviewDate").val(result.MeetingHistorySummaryTab[0].PlanerReviewDate);
                    $("#TextBoxMeetingReason").val(result.MeetingHistorySummaryTab[0].MeetingReason);
                    $("#TextBoxMemberAttendance").val(result.MeetingHistorySummaryTab[0].MemberAttendance);
                    $("#TextBoxMeetingId").val(result.MeetingHistorySummaryTab[0].MeetingId);
                    $("#TextBoxInformationPresented").val(result.MeetingHistorySummaryTab[0].InformationPresented);
                    $("#TextBoxInformationDiscussed").val(result.MeetingHistorySummaryTab[0].InformationDiscussed);
                    $("#DropDownTypeOfMeeting").select2('val', [result.MeetingHistorySummaryTab[0].TypeOfMeeting]);
                    $("#meetingHistoryModal").modal("show");
                }
            }
            else {
                showErrorMessage(result.Message);
            }

        },
        error: function (xhr) { HandleAPIError(xhr) }

    });
}
function createBtnMeeting(text) {
    let td = $("<td/>").addClass("td-actions");
    let updateBtn = $("<button/>").addClass("btn btn- sm greenColor").attr("type", "button").attr("aria-hidden", true).attr("onclick", 'EditMeetingHistoryTabDetails(event,' + text + ');');
    $(updateBtn).val(text).html('<i class="fa fa-pencil"  aria-hidden="true"></i>');
    let deleteBtn = $("<button/>").addClass("btn btn-sm redColor").attr("type", "button").attr("aria-hidden", true).attr("onclick", 'DeleteMeetingHistoryRecords(' + text + ');');
    $(deleteBtn).val(text).html('<i class="fa fa-trash-o" aria-hidden="true"></i>');
    $(td).append(updateBtn);
    $(td).append(deleteBtn);
    return td;
}
function clearMeetingHistorytext() {

    $("#TextBoxPlanerReviewDate").val("");
    $("#TextBoxMeetingReason").val("");
    $("#TextBoxMemberAttendance").val("");

    $("#TextBoxInformationPresented").val("");
    $("#TextBoxInformationDiscussed").val("");
    $("#DropDownTypeOfMeeting").val(null).trigger('change');

}
function DeleteMeetingHistoryRecords(id) {
    var json = [],
        item = {},
        tag = "Id";

    item[tag] = id;
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "GetMeetingHistory", LifePlanId: "", Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "deleteById" },
        url: GetAPIEndPoints("HANDLEMEETINGTAIL"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {

            showRecordSaved('Record Deleted');
            GetMeetingHistoryTabDetails();


        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}
//#endregion


//#region IndividualSafety
function InsertModifyIndividualSafe() {
    if (!validateIndividualSafeTab()) return;
    var json = [],
        item = {},
        tag;
    $('#imuIndividualSafe .IndividualSafe-control').each(function () {
        tag = $(this).attr('name').replace("TextIBox", "").replace("TextBox", "").replace("Checkbox", "").replace("DropDown", "").replace("Radio", "");
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
            else {
                item[tag] = jsonWrapperWithTimePicker(tag, this);
            }
        }
    });
    item["DocumentVersionId"] = $("#TextBoxDocumentVersionId").val();
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "IndividualSafe", Json: JSON.stringify(json), ReportedBy: reportedBy },
        url: GetAPIEndPoints("INSERTMODIFYLIFEPLANDETAIL"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                $("#TextBoxIndividualPlanOfProtectionID").val("");
                if ($("#TextBoxIndividualPlanOfProtectionID").val() == null || $("#TextBoxIndividualPlanOfProtectionID").val() == "") {
                    cleartext();
                    showRecordSaved("Record Saved.");

                }
                else {
                    showRecordSaved("Record Updated.");

                }
                clearIndividualSavetext();
                GetIndividualSafeTabDetails();

                $("#exampleModa3").modal("hide");
            }
            else {
                showErrorMessage(result.Message);
            }

        },

        error: function (xhr) { HandleAPIError(xhr) }
    });
}
function validateIndividualSafeTab() {
    var checked = null;
    $("#imuIndividualSafe .req_feild").each(function () {
        if ($(this).attr("type") == "radio") {
            var radio = $(this).attr("name");
            if ($('input[name=' + radio + ']:checked').length == 0) {
                $(this).parent().parent().parent().next().children().removeClass("hidden");
                $(this).focus();
                checked = false;
                return checked;
            }
        }
        else if ($(this).val() == "" || $(this).val() == "-1") {
            $(this).siblings("span.errorMessage").removeClass("hidden");
            $(this).focus();
            checked = false;
            return checked;
        }
    });
    if (checked == null) {
        return true;
    }
}
function GetIndividualSafeTabDetails() {
    var json = [],
        item = {},
        tag = "LifePlanId";
    if ($("#TextIBoxLifePlanId").val() != "") {
        item[tag] = $("#TextIBoxLifePlanId").val();
        json.push(item);
        $.ajax({
            type: "POST",
            data: { TabName: "GetIndividualSafe", LifePlanId: $("#TextIBoxLifePlanId").val(), Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "select" },
            dataType: 'json',
            url: GetAPIEndPoints("HANDLEINDIVIDUALSAFERECORDS"),
            headers: { 'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5" },
            success: function (result) {

                if (result.Success == true) {
                    var jsonStringyfy = JSON.stringify(result.IndividualSafeSummaryTab) == "null" ? "{}" : JSON.stringify(result.IndividualSafeSummaryTab);
                    $('#tblIndividualSafety').DataTable({
                        "stateSave": true,
                        "bDestroy": true,
                        "paging": true,
                        "searching": true,
                        "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
                        "aaData": JSON.parse(jsonStringyfy),
                        "columns": [{ "data": "GoalValuedOutcome" }, { "data": "ProviderAssignedGoal" }, { "data": "ProviderLocation" }, { "data": "ServicesType" }, { "data": "Frequency" }, { "data": "Quantity" },
                        { "data": "TimeFrame" }, { "data": "SpecialConsiderations" }, { "data": "Actions" }]
                    });
                    jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');
                }
                else {
                    showErrorMessage(result.Message);
                }

            },
            error: function (xhr) { HandleAPIError(xhr) }
        });
    }


}
function createBtnISafe(text) {
    let td = $("<td/>").addClass("td-actions");
    let updateBtn = $("<button/>").addClass("btn btn- sm greenColor").attr("type", "button").attr("aria-hidden", true).attr("onclick", 'EditIndividualSafeTabDetails(event,' + text + ');');
    $(updateBtn).val(text).html('<i class="fa fa-pencil"  aria-hidden="true"></i>');
    let deleteBtn = $("<button/>").addClass("btn btn-sm redColor").attr("type", "button").attr("aria-hidden", true).attr("onclick", 'DeleteIndividualSafeRecords(' + text + ');');
    $(deleteBtn).val(text).html('<i class="fa fa-trash-o" aria-hidden="true"></i>');
    $(td).append(updateBtn);
    $(td).append(deleteBtn);
    return td;
}
function EditIndividualSafeTabDetails(e, id) {
    var json = [],
        item = {},
        tag = "Id";

    item[tag] = id;
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "GetIndividualSafe", LifePlanId: "", Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "selectById" },
        url: GetAPIEndPoints("HANDLEINDIVIDUALSAFERECORDS"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                if (result.IndividualSafeSummaryTab.length > 0) {
                    debugger;
                    $(".section3 #TextBoxGoalValuedOutcome").val(result.IndividualSafeSummaryTab[0].GoalValuedOutcome);
                    $(".section3 #TextBoxProviderAssignedGoal").val(result.IndividualSafeSummaryTab[0].ProviderAssignedGoal);
                    $(".section3 #IndividualProviderLocation").select2('val', [result.IndividualSafeSummaryTab[0].ProviderLocation]);
                    $(".section3 #DropDownServicesTypeIPOP").select2('val', [result.IndividualSafeSummaryTab[0].ServicesType]);
                    $(".section3 #DropDownFrequencyIPOP").select2('val', [result.IndividualSafeSummaryTab[0].Frequency]);
                    $(".section3 #DropDownQuantityIPOP").select2('val', [result.IndividualSafeSummaryTab[0].Quantity]);
                    $(".section3 #DropDownTimeFrameIPOP").select2('val', [result.IndividualSafeSummaryTab[0].TimeFrame]);
                    $(".section3 #TextBoxSpecialConsiderations").val(result.IndividualSafeSummaryTab[0].SpecialConsiderations);
                    $(".section3 #TextBoxIndividualPlanOfProtectionID").val(result.IndividualSafeSummaryTab[0].IndividualPlanOfProtectionID);
                    $(".section3 #exampleModa3").modal("show");
                }
            }
            else {
                showErrorMessage(result.Message);
            }

        },
        error: function (xhr) { HandleAPIError(xhr) }

    });
}
function DeleteIndividualSafeRecords(id) {
    var json = [],
        item = {},
        tag = "Id";

    item[tag] = id;
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "GetIndividualSafe", LifePlanId: "", Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "deleteById" },
        url: GetAPIEndPoints("HANDLEINDIVIDUALSAFERECORDS"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {

            showRecordSaved('Record Deleted');
            GetIndividualSafeTabDetails();


        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}
function clearIndividualSavetext() {
    $(".section3 #TextBoxGoalValuedOutcome").val("");
    $(".section3 #TextBoxProviderAssignedGoal").val("");
    $(".section3 #IndividualProviderLocation").val(null).trigger('change');
    $(".section3 #DropDownServicesTypeIPOP").val(null).trigger('change');
    $(".section3 #DropDownFrequencyIPOP").val(null).trigger('change');
    $(".section3 #DropDownQuantityIPOP").val(null).trigger('change');
    $(".section3 #DropDownTimeFrameIPOP").val(null).trigger('change');
    $(".section3 #TextBoxSpecialConsiderations").val("");
    $(".section3 #TextBoxIndividualPlanOfProtectionID").val("");

}
//#endregion

//#region AssessmentNarrativeSummary
function validateAssessmentNarrativeSummary() {
    if (($("#TextAreaMyHome").val() == "" && $("#TextAreaMyWork").val() == "" && $("#TextAreaMyHealthAndMedication").val() == "" && $("#TextAreaMyRelationships").val() == "")) {
        showErrorMessage("Please enter atleast single assessment narrative summary value.")
        return false;
    }
    return true;
}
function GetAssessmentNarrativeSummaryTabDetails() {

    var json = [],
        item = {},
        tag = "LifePlanId";
    if ($("#TextAreaLifePlanId").val() != null || $("#TextAreaLifePlanId").val() != "") {
        item[tag] = $("#TextAreaLifePlanId").val();
        json.push(item);
        $.ajax({
            type: "POST",
            data: { TabName: "GetAssessmentNarrativeSummaryTab", LifePlanId: $("#TextAreaLifePlanId").val(), Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "select" },
            dataType: 'json',
            url: GetAPIEndPoints("HANDLEASSESSMENTNARRATIVESUMMARY"),
            headers: { 'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5" },
            success: function (result) {
                if (result.Success == true) {
                    if (result.AssessmentNarrativeSummaryTab != null) {
                        if (result.AssessmentNarrativeSummaryTab.length == 1) {
                            $("#TextAreaMyHome").val(result.AssessmentNarrativeSummaryTab[0].MyHome);
                            $("#TextAreaMyWork").val(result.AssessmentNarrativeSummaryTab[0].MyWork);
                            $("#TextAreaMyHealthAndMedication").val(result.AssessmentNarrativeSummaryTab[0].MyHealthAndMedication);
                            $("#TextAreaMyRelationships").val(result.AssessmentNarrativeSummaryTab[0].MyRelationships);
                            $("#TextBoxAssessmentNarrativeSummaryId").val(result.AssessmentNarrativeSummaryTab[0].AssessmentNarrativeSummaryId);
                        }
                    }
                }
                else {
                    showErrorMessage(result.Message);
                }


            },
            error: function (xhr) { HandleAPIError(xhr) }
        });
    }
}
function createBtn(text) {
    let td = $("<td/>").addClass("td-actions");
    let updateBtn = $("<button/>").addClass("btn btn- sm greenColor").attr("type", "button").attr("aria-hidden", true).attr("onclick", 'EditAssessmentNarrativeSummary(event,' + text + ');');
    $(updateBtn).val(text).html('<i class="fa fa-pencil"  aria-hidden="true"></i>');
    let deleteBtn = $("<button/>").addClass("btn btn-sm redColor").attr("type", "button").attr("aria-hidden", true).attr("onclick", 'DeleteSectionOneRecord(' + text + ');');
    $(deleteBtn).val(text).html('<i class="fa fa-trash-o" aria-hidden="true"></i>');
    $(td).append(updateBtn);
    $(td).append(deleteBtn);
    return td;
}
function InsertModifyAssessmentNarrativeSummaryTab() {
    if ($("#btnAssessmentNarrativesummary").text() == "Edit") {
        $("#btnAssessmentNarrativesummary").text("OK");
        $('#lfuFromAssessmentNarrativeSummary .LifePlanEnable').attr("disabled", false);
        return;
    }
    if (!validateAssessmentNarrativeSummary()) return;
    var json = [],
        item = {},
        tag;
    $('#lfuFromAssessmentNarrativeSummary .gen-control').each(function () {
        tag = $(this).attr('name').replace("TextArea", "").replace("TextBox", "").replace("Checkbox", "").replace("DropDown", "").replace("Radio", "").replace("TextBoxx", "");
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
            else {
                item[tag] = jsonWrapperWithTimePicker(tag, this);
            }
        }
    });
    item["DocumentVersionId"] = $("#TextBoxDocumentVersionId").val();
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "AssessmentNarrativeSummary", Json: JSON.stringify(json), ReportedBy: reportedBy },
        url: GetAPIEndPoints("INSERTMODIFYLIFEPLANDETAIL"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                if ($("#btnAssessmentNarrativesummary").text() != "Edit") {
                    if ($("#TextBoxAssessmentNarrativeSummaryId").val() == null || $("#TextBoxAssessmentNarrativeSummaryId").val() == "") {

                        showRecordSaved("Record Saved.");
                    }
                    else {
                        showRecordSaved("Record Updated.");
                    }
                }
                GetAssessmentNarrativeSummaryTabDetails();
                if ($("#btnAssessmentNarrativesummary").text() == "Edit") {
                    $("#btnAssessmentNarrativesummary").text("OK");
                    $('#lfuFromAssessmentNarrativeSummary .LifePlanEnable').attr("disabled", false);

                }
                else {

                    $("#btnAssessmentNarrativesummary").text("Edit");
                    $('#lfuFromAssessmentNarrativeSummary .LifePlanEnable').attr("disabled", true);


                }
            }
            else {
                showErrorMessage(result.Message);
            }


        },
        error: function (xhr) { HandleAPIError(xhr) }
    });

}
function cleartext() {
    if ($("#btnAssessmentNarrativesummary").text() == "Ok") {
        $("#TextAreaMyHome").val("");
        $("#TextAreaMyWork").val("");
        $("#TextAreaMyHealthAndMedication").val("");
        $("#TextAreaMyRelationships").val("");
        $("#TextBoxAssessmentNarrativeSummaryId").val("");
    }
    $("#collapseEight").removeClass("collapse show").addClass("collapse");
}
function hideSections(Id) {
    
    $("#"+Id).removeClass("collapse show").addClass("collapse");
}
//#endregion    

//#region OutcomesStrategies
function validateOutcomesStrategies() {
    var checked = null;
    $("#lfuFromOutcomesStrategies .req_feild").each(function () {
        if ($(this).attr("type") == "radio") {
            var radio = $(this).attr("name");
            if ($('input[name=' + radio + ']:checked').length == 0) {
                $(this).parent().parent().parent().next().children().removeClass("hidden");
                $(this).focus();
                checked = false;
                return checked;
            }
        }
        else if ($(this).val() == "" || $(this).val() == "-1") {
            $(this).siblings("span.errorMessage").removeClass("hidden");
            $(this).focus();
            checked = false;
            return checked;
        }
    });
    if (checked == null) {
        return true; 
    }
}
function validateSupportOutcomesStrategies() {
    if ($('#tblStrategiesOutcomesSupportStrategies input[name="RadioSuggestedSupportStrategie"]:checked').length < 1) {
        showErrorMessage("Please select atleast on checkbox.");
        return false;
    }
    return true;
}
function GetOutcomesStrategiesTabDetails() {

    var json = [],
        item = {},
        tag = "LifePlanId";
    if ($("#TextBoxOutcomesStrategiesLifePlanId").val() != "" || $("#TextBoxOutcomesStrategiesLifePlanId").val() != null) {

        item[tag] = $("#TextBoxOutcomesStrategiesLifePlanId").val();
        json.push(item);
        $.ajax({
            type: "POST",
            data: { TabName: "GetOutcomesSupportStrategies", LifePlanId: $("#TextBoxOutcomesStrategiesLifePlanId").val(), Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "select" },
            dataType: 'json',
            url: GetAPIEndPoints("HANDLEOUTCOMESSTRATEGIES"),
            headers: { 'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5" },
            success: function (result) {


                if (result.Success == true) {
                    var jsonStringyfy = JSON.stringify(result.OutcomesSupportStrategiesTab) == "null" ? "{}" : JSON.stringify(result.OutcomesSupportStrategiesTab);
                    $('#tblOutcomesSupportStrategies').DataTable({
                        "stateSave": true,
                        "bDestroy": true,
                        "paging": true,
                        "searching": true,
                        "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
                        "aaData": JSON.parse(jsonStringyfy),
                        "columns": [{ "data": "CqlPomsGoal" }, { "data": "CcoGoal" }, { "data": "ProviderAssignedGoal" }, { "data": "ProviderLocation" }, { "data": "ServicesType" }, { "data": "Frequency" },
                        { "data": "Quantity" }, { "data": "TimeFrame" }, { "data": "SpecialConsiderations" }, { "data": "Actions" }]
                    });
                    jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');
                }
                else {
                    showErrorMessage(result.Message);
                }

            },
            error: function (xhr) { HandleAPIError(xhr) }
        });
    }


}
function EditOutcomesStrategies(e, id) {

    var json = [],
        item = {},
        tag = "Id";

    item[tag] = id;
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "GetOutcomesSupportStrategies", LifePlanId: "", Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "selectById" },
        url: GetAPIEndPoints("HANDLEOUTCOMESSTRATEGIES"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                if (result.OutcomesSupportStrategiesTab.length == 1) {
                    $("#DropDownCqlPomsGoal").select2('val', [result.OutcomesSupportStrategiesTab[0].CqlPomsGoal]);
                    $("#TextBoxCcoGoal").val(result.OutcomesSupportStrategiesTab[0].CcoGoal);
                    $("#TextBoxProviderAssignedGoal").val(result.OutcomesSupportStrategiesTab[0].ProviderAssignedGoal);
                    $("#DropDownProviderLocation").select2('val', [result.OutcomesSupportStrategiesTab[0].ProviderLocation]);
                    $("#DropDownServicesType").select2('val', [result.OutcomesSupportStrategiesTab[0].ServicesType]);
                    $("#DropDownFrequency").select2('val', [result.OutcomesSupportStrategiesTab[0].Frequency]);
                    $("#DropDownQuantity").select2('val', [result.OutcomesSupportStrategiesTab[0].Quantity]);
                    $("#DropDownTimeFrame").select2('val', [result.OutcomesSupportStrategiesTab[0].TimeFrame]);
                    $("#TextBoxSpecialConsiderations").val(result.OutcomesSupportStrategiesTab[0].SpecialConsiderations);
                    $("#TextBoxSupportStrategieId").val(result.OutcomesSupportStrategiesTab[0].SupportStrategieId);

                }
                $("#outcomeSourceModal").modal("show");
            }
            else {
                showErrorMessage(result.Message);
            }

        },
        error: function (xhr) { HandleAPIError(xhr) }

    });
}
function InsertModifyOutcomesStrategiesExported() {
    if (!validateSupportOutcomesStrategies()) return;
    var  JsonFirstTable = [];

    if ($('#tblStrategiesOutcomesSupportStrategies input[name="RadioSuggestedSupportStrategie"]:checked').length >= 1) {
        var checkedRowLength = $('#tblStrategiesOutcomesSupportStrategies input[type="checkbox"]:checked').length;
        // for (i = 0; i < checkedRowLength; i++) {
        $('#tblStrategiesOutcomesSupportStrategies input[name="RadioSuggestedSupportStrategie"]:checked').each(function () {
            //var row = $(this).closet("tr")[0];
            var firstTable = {};
            //  firstTable["MedicalDiagnosisId"] = $("#MedicalDiagnosis tbody tr:eq(" + i + ") td:eq(0)").text();
            firstTable["CqlPomsGoal"] = $("#tblStrategiesOutcomesSupportStrategies tbody tr:eq(" + $(this).val() + ") td:eq(14)").text();
            firstTable["CcoGoal"] = $("#tblStrategiesOutcomesSupportStrategies tbody tr:eq(" + $(this).val() + ") td:eq(2)").text();
            firstTable["ProviderAssignedGoal"] = $("#tblStrategiesOutcomesSupportStrategies tbody tr:eq(" + $(this).val() + ") td:eq(3)").text();
            firstTable["ProviderLocation"] = $("#tblStrategiesOutcomesSupportStrategies tbody tr:eq(" + $(this).val() + ") td:eq(15)").text();
            firstTable["ServicesType"] = $("#tblStrategiesOutcomesSupportStrategies tbody tr:eq(" + $(this).val() + ") td:eq(10)").text();
            firstTable["Frequency"] = $("#tblStrategiesOutcomesSupportStrategies tbody tr:eq(" + $(this).val() + ") td:eq(11)").text();
            firstTable["Quantity"] = $("#tblStrategiesOutcomesSupportStrategies tbody tr:eq(" + $(this).val() + ") td:eq(12)").text();
            firstTable["TimeFrame"] = $("#tblStrategiesOutcomesSupportStrategies tbody tr:eq(" + $(this).val() + ") td:eq(13)").text();
            firstTable["SpecialConsiderations"] = $("#tblStrategiesOutcomesSupportStrategies tbody tr:eq(" + $(this).val() + ") td:eq(9)").text();
            firstTable["DocumentVersionId"] = $("#TextBoxDocumentVersionId").val();
            firstTable["LifePlanId"] = $("#TextBoxLifePlanId").val();
            firstTable["SupportStrategieId"] = $("#TextBoxSupportStrategieId").val();
            JsonFirstTable.push(firstTable);
            // }

        });

    }
    $.ajax({
        type: "POST",
        data: { TabName: "InsertOutcomesSupportStrategies", Json: JSON.stringify(JsonFirstTable), ReportedBy: reportedBy },
        url: GetAPIEndPoints("INSERTMODIFYLIFEPLANDETAIL"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                if ($("#TextBoxSupportStrategieId").val() == null || $("#TextBoxSupportStrategieId").val() == "") {

                    showRecordSaved("Record Saved.");

                }
                else {
                    showRecordSaved("Record Updated.");

                }
                cleartextOutcomesStrategies();
                GetOutcomesStrategiesTabDetails();
                $("#exampleModa2").modal("hide");
            }
            else {
                showErrorMessage(result.Message);
            }


        },
        error: function (xhr) { HandleAPIError(xhr) }
    });

}
function InsertModifyOutcomesStrategiesTab() {
    if (!validateOutcomesStrategies()) return;
    var json = [],
        item = {},
        tag;

    $('#lfuFromOutcomesStrategies .gen-control').each(function () {
        tag = $(this).attr('name').replace("TextBoxOutcomesStrategies", "").replace("TextBox1", "").replace("TextArea", "").replace("TextBox", "").replace("DropDown", "").replace("Checkbox", "").replace("Radio", "");
        if ($(this).hasClass("req_feild")) {
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
            else {
                item[tag] = jsonWrapperWithTimePicker(tag, this);
            }
        }
    });
    item["DocumentVersionId"] = $("#TextBoxDocumentVersionId").val();
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "InsertOutcomesSupportStrategies", Json: JSON.stringify(json), ReportedBy: reportedBy },
        url: GetAPIEndPoints("INSERTMODIFYLIFEPLANDETAIL"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                if ($("#TextBoxSupportStrategieId").val() == null || $("#TextBoxSupportStrategieId").val() == "") {

                    showRecordSaved("Record Saved.");

                }
                else {
                    showRecordSaved("Record Updated.");

                }
                cleartextOutcomesStrategies();
                GetOutcomesStrategiesTabDetails();
                $("#outcomeSourceModal").modal("hide");
            }
            else {
                showErrorMessage(result.Message);
            }


        },
        error: function (xhr) { HandleAPIError(xhr) }
    });

}
function DeleteOutcomesStrategiesRecord(id) {
    var json = [],
        item = {},
        tag = "Id";

    item[tag] = id;
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "GetOutcomesSupportStrategies", LifePlanId: "", Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "deleteById" },
        url: GetAPIEndPoints("HANDLEOUTCOMESSTRATEGIES"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {

            GetOutcomesStrategiesTabDetails();
            showRecordSaved("Record Deleted.");


        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}
function cleartextOutcomesStrategies() {
    $(".section2 #DropDownCqlPomsGoal").val(null).trigger('change');  //$('#mySelect2').val(null).trigger('change');
    $(".section2 #DropDownServicesType").val(null).trigger('change');
    $(".section2 #DropDownFrequency").val(null).trigger('change');
    $(".section2 #DropDownQuantity").val(null).trigger('change');
    $(".section2 #DropDownTimeFrame").val(null).trigger('change');
    $(".section2 #TextBoxCcoGoal").val("");
    $(".section2 #TextBoxProviderAssignedGoal").val("");
    $(".section2 #DropDownProviderLocation").val(null).trigger('change');
    $(".section2 #TextBoxSpecialConsiderations").val("");
    $(".section2 #TextBoxSupportStrategieId").val("");
}
function createBtnOutcomesStrategies(text) {
    let td = $("<td/>").addClass("td-actions");
    let updateBtn = $("<button/>").addClass("btn btn- sm greenColor").attr("type", "button").attr("aria-hidden", true).attr("onclick", 'EditOutcomesStrategies(event,' + text + ');');
    $(updateBtn).val(text).html('<i class="fa fa-pencil"  aria-hidden="true"></i>');
    let deleteBtn = $("<button/>").addClass("btn btn-sm redColor").attr("type", "button").attr("aria-hidden", true).attr("onclick", 'DeleteOutcomesStrategiesRecord(' + text + ');');
    $(deleteBtn).val(text).html('<i class="fa fa-trash-o" aria-hidden="true"></i>');
    $(td).append(updateBtn);
    $(td).append(deleteBtn);
    return td;
}
//#endregion

//#region  HCBSWaiver
function validateHCBSWaiver() {
    var checked = null;
    var startDate = $("#TextBoxEffectiveFrom").val();
    var endDate = $("#TextBoxEffectiveTo").val();

    if ((Date.parse(startDate) >= Date.parse(endDate))) {
        //alert("End date should be greater than Start date");
        showErrorMessage("End date should be greater than Start date");
        $("#TextBoxEffectiveTo").val("");
        checked = false;
        return checked;
    }
    $("#lfuFromHCBSWaiver .req_feild").each(function () {
        if ($(this).attr("type") == "radio") {
            var radio = $(this).attr("name");
            if ($('input[name=' + radio + ']:checked').length == 0) {
                $(this).parent().parent().parent().next().children().removeClass("hidden");
                $(this).focus();
                checked = false;
                return checked;
            }
        }
        else if ($(this).val() == "" || $(this).val() == "-1") {
            $(this).siblings("span.errorMessage").removeClass("hidden");
            $(this).focus();
            checked = false;
            return checked;
        }
    });
    if (checked == null) {
        return true;
    }
}
function GetHCBSWaiverTabDetails() {

    var json = [],
        item = {},
        tag = "LifePlanId";
    if ($("#TextBoxHCBSLifePlanId").val() != "") {
        item[tag] = $("#TextBoxHCBSLifePlanId").val();
        json.push(item);
        $.ajax({
            type: "POST",
            data: { TabName: "GetMedicaidStatePlanAuthorizedServies", LifePlanId: $("#TextBoxHCBSLifePlanId").val(), Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "select" },
            dataType: 'json',
            url: GetAPIEndPoints("HANDLEHCBSWaiver"),
            headers: { 'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5" },
            success: function (result) {


                if (result.Success == true) {
                    var jsonStringyfy = JSON.stringify(result.HCBSWaiverTab) == "null" ? "{}" : JSON.stringify(result.HCBSWaiverTab);
                    $('#tblHCBSWaiver').DataTable({
                        "stateSave": true,
                        "bDestroy": true,
                        "paging": true,
                        "searching": true,
                        "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
                        "aaData": JSON.parse(jsonStringyfy),
                        "aoColumns": [{ "mData": "Code" }, { "data": "FacilityName" }, { "mData": "CombinedDate" }, { "mData": "Unit" }, { "mData": "Comments" }, { "mData": "Actions" }]
                    });
                    jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');
                }
                else {
                    showErrorMessage(result.Message);
                }

            },
            error: function (xhr) { HandleAPIError(xhr) }
        });
    }


}
function EditHCBSWaiver(e, id) {

    var json = [],
        item = {},
        tag = "Id";

    item[tag] = id;
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "GetMedicaidStatePlanAuthorizedServies", LifePlanId: "", Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "selectById" },
        url: GetAPIEndPoints("HANDLEHCBSWaiver"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                if (result.HCBSWaiverTab.length > 0) {
                    InitalizeDateControls();
                    $(".section4 #DropDownCode").select2('val', [result.HCBSWaiverTab[0].Code]);
                    $(".section4 #TextBoxUnit").val(result.HCBSWaiverTab[0].Unit);
                    $(".section4 #TextBoxFirstName").val(result.HCBSWaiverTab[0].FirstName);
                    $(".section4 #TextBoxLastName").val(result.HCBSWaiverTab[0].LastName);
                    $(".section4 #TextBoxFacilityCode").val(result.HCBSWaiverTab[0].FacilityCode);
                    $(".section4 #DropDownFacilityName").select2('val', [result.HCBSWaiverTab[0].FacilityName]);
                    $(".section4 #TextBoxEffectiveFrom").val(result.HCBSWaiverTab[0].EffectiveFrom);
                    $(".section4 #TextBoxEffectiveTo").val(result.HCBSWaiverTab[0].EffectiveTo);
                    $(".section4 #TextAreaDescription").val(result.HCBSWaiverTab[0].Description);
                    $(".section4 #TextAreaComments").val(result.HCBSWaiverTab[0].Comments);
                    $(".section4 #TextBoxMedicaidStatePlanAuthorizedServiesId").val(result.HCBSWaiverTab[0].MedicaidStatePlanAuthorizedServiesId);
                }

                $("#exampleModa4").modal("show");
            }
            else {
                showErrorMessage(result.Message);
            }


        },
        error: function (xhr) { HandleAPIError(xhr) }

    });
}
function InsertModifyHCBSWaiverTab() {
    if (!validateHCBSWaiver()) return;
    var json = [],
        item = {},
        tag;

    $('#lfuFromHCBSWaiver .gen-control').each(function () {
        tag = $(this).attr('name').replace("TextBoxHCBS", "").replace("TextBox1", "").replace("TextArea", "").replace("TextBox", "").replace("DropDown", "").replace("Checkbox", "").replace("Radio", "");
        if ($(this).hasClass("req_feild")) {
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
            else {
                item[tag] = jsonWrapperWithTimePicker(tag, this);
            }
        }
    });
    item["DocumentVersionId"] = $("#TextBoxDocumentVersionId").val();
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "InsertHCBSWaiver", Json: JSON.stringify(json), ReportedBy: reportedBy },
        url: GetAPIEndPoints("INSERTMODIFYLIFEPLANDETAIL"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                if ($("#TextBoxMedicaidStatePlanAuthorizedServiesId").val() == null || $("#TextBoxMedicaidStatePlanAuthorizedServiesId").val() == "") {

                    showRecordSaved("Record Saved.");

                }
                else {
                    showRecordSaved("Record Updated.");

                }

                cleartextHCBSWaiver();
                GetHCBSWaiverTabDetails();
                $("#exampleModa4").modal("hide");
            }
            else {
                showErrorMessage(result.Message);
            }



        },
        error: function (xhr) { HandleAPIError(xhr) }
    });

}
function DeleteHCBSWaiverRecord(id) {
    var json = [],
        item = {},
        tag = "Id";

    item[tag] = id;
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "GetMedicaidStatePlanAuthorizedServies", LifePlanId: "", Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "deleteById" },
        url: GetAPIEndPoints("HANDLEHCBSWaiver"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {

            GetHCBSWaiverTabDetails();
            showRecordSaved("Record Deleted.");


        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}
function cleartextHCBSWaiver() {

    $(".section4 #DropDownCode").val(null).trigger('change');
    $(".section4 #TextBoxUnit").val("");
    $(".section4 #TextBoxFirstName").val("");
    $(".section4 #TextBoxLastName").val("");
    $(".section4 #TextBoxFacilityCode").val("");
    $(".section4 #DropDownFacilityName").val(null).trigger('change');
    $(".section4 #TextBoxEffectiveFrom").val("");
    $(".section4 #TextBoxEffectiveTo").val("");
    $(".section4 #TextAreaDescription").val("");
    $(".section4 #TextAreaComments").val("");
    $(".section4 #TextBoxMedicaidStatePlanAuthorizedServiesId").val("");



}
function createBtnHCBSWaiver(text) {
    let td = $("<td/>").addClass("td-actions");
    let updateBtn = $("<button/>").addClass("btn btn- sm greenColor").attr("type", "button").attr("aria-hidden", true).attr("onclick", 'EditHCBSWaiver(event,' + text + ');');
    $(updateBtn).val(text).html('<i class="fa fa-pencil"  aria-hidden="true"></i>');
    let deleteBtn = $("<button/>").addClass("btn btn-sm redColor").attr("type", "button").attr("aria-hidden", true).attr("onclick", 'DeleteHCBSWaiverRecord(' + text + ');');
    $(deleteBtn).val(text).html('<i class="fa fa-trash-o" aria-hidden="true"></i>');
    $(td).append(updateBtn);
    $(td).append(deleteBtn);
    return td;
}
//#endregion


//#region Fundal&NaturalCommunityResources
function validateInsertModifyFundalNaturalCommunityResources() {
    var checked = null;
    $("#lfuFromFundalNaturalCommunityResources .req_feild").each(function () {
        if ($(this).attr("type") == "radio") {
            var radio = $(this).attr("name");
            if ($('input[name=' + radio + ']:checked').length == 0) {
                $(this).parent().parent().parent().next().children().removeClass("hidden");
                $(this).focus();
                checked = false;
                return checked;
            }
        }
        else if ($(this).val() == "" || $(this).val() == "-1") {
            $(this).siblings("span.errorMessage").removeClass("hidden");
            checked = false;
            return checked;
        }
    });
    if (checked == null) {
        return true;
    }
}
function GetFundalNaturalCommunityResourcesTabDetails() {

    var json = [],
        item = {},
        tag = "LifePlanId";
    if ($("#TextBoxFundalResourcesLifePlanId").val() != "") {
        item[tag] = $("#TextBoxFundalResourcesLifePlanId").val();
        json.push(item);
        $.ajax({
            type: "POST",
            data: { TabName: "GetFundalNaturalCommunityResources", LifePlanId: $("#TextBoxFundalResourcesLifePlanId").val(), Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "select" },
            dataType: 'json',
            url: GetAPIEndPoints("HANDLEFUNDALNATURALCOMMUNITYRESOURCES"),
            headers: {
                'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
            },
            success: function (result) {
                if (result.Success == true) {
                    var jsonStringyfy = JSON.stringify(result.fundalNaturalCommunityResourcesTab) == "null" ? "{}" : JSON.stringify(result.fundalNaturalCommunityResourcesTab);
                    $('#tblFundalNaturalCommunityResources').DataTable({
                        "stateSave": true,
                        "bDestroy": true,
                        "paging": true,
                        "searching": true,
                        "autoWidth": false,
                        "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
                        "aaData": JSON.parse(jsonStringyfy),
                        "columns": [{ "data": "Name" }, { "data": "Role" }, { "data": "AddressOne" }, { "data": "Phone" }, { "data": "Actions" }]
                    });
                    jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');
                    BindContactControlsDropDown(result.fundalNaturalCommunityResourcesTab);
                }
                else {
                    showErrorMessage(reslut.Message);
                }
                
            },
            error: function (xhr) { HandleAPIError(xhr) }
        });
    }


}
function EditFundalNaturalCommunityResources(e, id) {

    var json = [],
        item = {},
        tag = "Id";

    item[tag] = id;
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "GetFundalNaturalCommunityResources", LifePlanId: "", Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "selectById" },
        url: GetAPIEndPoints("HANDLEFUNDALNATURALCOMMUNITYRESOURCES"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                if (result.fundalNaturalCommunityResourcesTab.length > 0) {
                    $(".section5 #TextBoxFundalResourcesFirstName").val(result.fundalNaturalCommunityResourcesTab[0].FirstName);
                    $(".section5 #TextBoxFundalResourcesLastName").val(result.fundalNaturalCommunityResourcesTab[0].LastName);
                    $(".section5 #TextBoxRole").val(result.fundalNaturalCommunityResourcesTab[0].Role);
                    $(".section5 #TextBoxFundalResourcesPhone").val(result.fundalNaturalCommunityResourcesTab[0].Phone);
                    $(".section5 #TextAreaAddressOne").val(result.fundalNaturalCommunityResourcesTab[0].AddressOne);
                    $(".section5 #TextAreaAddressTwo").val(result.fundalNaturalCommunityResourcesTab[0].AddressTwo);
                    $(".section5 #TextBoxFundalCity").val(result.fundalNaturalCommunityResourcesTab[0].City);
                    $(".section5 #TextBoxFundalState").val(result.fundalNaturalCommunityResourcesTab[0].State);
                    $(".section5 #TextBoxFundalResourcesZip").val(result.fundalNaturalCommunityResourcesTab[0].Zip);
                    $(".section5 #TextBoxFundalNaturalCommunityResourcesId").val(result.fundalNaturalCommunityResourcesTab[0].FundalNaturalCommunityResourcesId);
                }

                $("#exampleModa5").modal("show");
            }
            else {
                showErrorMessage(result.Message);
            }

        },
        error: function (xhr) { HandleAPIError(xhr) }

    });
}
function InsertModifyFundalNaturalCommunityResourcesTab() {
    if (!validateInsertModifyFundalNaturalCommunityResources()) return;
    var json = [],
        item = {},
        tag;

    $('#lfuFromFundalNaturalCommunityResources .gen-control').each(function () {
        tag = $(this).attr('name').replace("TextBoxFundalResources", "").replace("DropDownFundal", "").replace("TextArea", "").replace("TextBox", "").replace("DropDown", "").replace("Checkbox", "").replace("Radio", "");
        if ($(this).hasClass("req_feild")) {
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
            else {
                item[tag] = jsonWrapperWithTimePicker(tag, this);
            }
        }
    });
    item["DocumentVersionId"] = $("#TextBoxDocumentVersionId").val();
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "InsertFundalNaturalCommunityResources", Json: JSON.stringify(json), ReportedBy: reportedBy },
        url: GetAPIEndPoints("INSERTMODIFYLIFEPLANDETAIL"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                if ($("#TextBoxFundalNaturalCommunityResourcesId").val() == null || $("#TextBoxFundalNaturalCommunityResourcesId").val() == "") {

                    showRecordSaved("Record Saved.");

                }
                else {
                    showRecordSaved("Record Updated.");

                }
                cleartextFundalNaturalCommunityResources();
                GetFundalNaturalCommunityResourcesTabDetails();
                $("#exampleModa5").modal("hide");
            }
            else {
                showErrorMessage(result.Message);
            }


        },
        error: function (xhr) { HandleAPIError(xhr) }
    });

}
function DeleteFundalNaturalCommunityResourcesRecord(id) {
    var json = [],
        item = {},
        tag = "Id";

    item[tag] = id;
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "GetFundalNaturalCommunityResources", LifePlanId: "", Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "deleteById" },
        url: GetAPIEndPoints("HANDLEFUNDALNATURALCOMMUNITYRESOURCES"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {

            GetFundalNaturalCommunityResourcesTabDetails();
            showRecordSaved("Record Deleted.");


        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}
function cleartextFundalNaturalCommunityResources() {

    $(".section5 #TextBoxFundalResourcesFirstName").val("");
    $(".section5 #TextBoxFundalResourcesLastName").val("");
    $(".section5 #TextBoxRole").val("");
    $(".section5 #TextBoxFundalResourcesPhone").val("");
    $(".section5 #TextAreaAddressOne").val("");
    $(".section5 #TextAreaAddressTwo").val("");
    $(".section5 #TextBoxFundalCity").val("");
    $(".section5 #TextBoxFundalState").val("");
    $(".section5 #TextBoxFundalResourcesZip").val("");
    $(".section5 #TextBoxFundalNaturalCommunityResourcesId").val("");

}
function createBtnFundalNaturalCommunityResources(text) {
    let td = $("<td/>").addClass("td-actions");
    let updateBtn = $("<button/>").addClass("btn btn- sm greenColor").attr("type", "button").attr("aria-hidden", true).attr("onclick", 'EditFundalNaturalCommunityResources(event,' + text + ');');
    $(updateBtn).val(text).html('<i class="fa fa-pencil"  aria-hidden="true"></i>');
    let deleteBtn = $("<button/>").addClass("btn btn-sm redColor").attr("type", "button").attr("aria-hidden", true).attr("onclick", 'DeleteFundalNaturalCommunityResourcesRecord(' + text + ');');
    $(deleteBtn).val(text).html('<i class="fa fa-trash-o" aria-hidden="true"></i>');
    $(td).append(updateBtn);
    $(td).append(deleteBtn);
    return td;
}
//#endregion

//#region SaveDraftRegion
function SaveDraft() {
    if (!ValidateDraft()) return;
    var json = [],
        item = {};
    $.ajax({
        type: "POST",
        data: { TabName: "CreateNewVersion", LifePlanId: $("#TextBoxLifePlanId").val(), DocumentVersionId: $("#TextBoxDocumentVersionId").val(), ReportedBy: reportedBy },
        url: GetAPIEndPoints("HANDLELIFEPLANVERSIONING"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {

                NewVersioncreated(result);
            }
            else {
                showErrorMessage(result.Message);
            }

        },

        error: function (xhr) { HandleAPIError(xhr) }
    });

}
function NewVersioncreated(response) {
    if (response.Success == true) {
        if (response.AllTab[0].LifePlanId > 0) {
            showRecordSaved("New version created successfully");
            $('#TextBoxLifePlanId').val(response.AllTab[0].LifePlanId);
            $('#TextBoxDocumentVersionId').val(response.AllTab[0].DocumentVersionId);
            $('#labelLifePlanStatus').text(response.AllTab[0].DocumentStatus);
            $('#labelDocumentVersion').text(Number(response.AllTab[0].DocumentVersion).toFixed(1));
            $('#TextMBoxLifePlanId').val(response.AllTab[0].LifePlanId);
            $('#TextBoxOutcomesStrategiesLifePlanId').val(response.AllTab[0].LifePlanId);
            $('#TextIBoxLifePlanId').val(response.AllTab[0].LifePlanId);
            $('#TextBoxHCBSLifePlanId').val(response.AllTab[0].LifePlanId);
            $('#TextBoxFundalResourcesLifePlanId').val(response.AllTab[0].LifePlanId);
            $('#TextAreaLifePlanId').val(response.AllTab[0].LifePlanId);
            $("#TextBoxLifePlanId").val(response.AllTab[0].LifePlanId);

            $("#btnSaveAsNew").addClass("hidden");
            $("#btnPrintPDf").show();
            $("#btnPublishVersion").show();
            $('#imuLifePlan .LifePlan-control').attr("disabled", true);
            $(".editRecord").text("Edit");
            $(".editRecord").show();
            $(".addModal-2").show();
            $(".addModal-3").show();
            $(".greenColor").prop("disabled", false);
            $(".redColor").prop("disabled", false);
            $('#lfuFromAssessmentNarrativeSummary .LifePlanEnable').attr("disabled", true);

            ShowHideSubmitReviewButton(response.AllTab[0].Status);
            GetAssessmentNarrativeSummaryTabDetails();
            GetMeetingHistoryTabDetails();
            GetIndividualSafeTabDetails();
            GetOutcomesStrategiesTabDetails();
            GetHCBSWaiverTabDetails();
            GetFundalNaturalCommunityResourcesTabDetails();
            GetLifePlanTabDetails();
            changeNewURLParameters(response);
        }

    }
}
function changeNewURLParameters(response) {
    var currentURL = $(location).attr("href");
    if (currentURL.indexOf('?') > -1) {
        var newURL = new URL(currentURL),
            changeLifePlanId = newURL.searchParams.set("LifePlanId", response.AllTab[0].LifePlanId),
            changeDocumentVersionId = newURL.searchParams.set("DocumentVersionId", response.AllTab[0].DocumentVersionId);
        history.pushState(null, 'Life Plan Template', newURL.href);
    }
}
function ValidateDraft() {
    if ($("#ExistingDraft").text() != "") {
        showErrorMessage("Already a draft exists for the life plan.");
        return false;
    }
    return true;
}
//#endregion


//#region CreatePublishVersion
function CreatePublishVersion() {
    $.ajax({
        type: "GET",
        url: "https://staging-api.cx360.net/api/Client/GetLifePlansDocumentPath",
        headers: {
            'TOKEN': token,
        },
        success: function (result) {
            if (result.length > 0) {
                PublishVersionAfterPathResponse(result);
            }
            else {
                showErrorMessage(result.Message);
            }

        },

        error: function (xhr) { HandleAPIError(xhr) }
    });
   
}
function PublishVersionAfterPathResponse(result) {
    var data = {
        FilePath: result[0].FilePath, TabName: "PublishLifePlanVersion", LifePlanId: $(".lifePlanId").val(), DocumentVersionId: $("#TextBoxDocumentVersionId").val(), IndividualName: $("#DropDownClientId").find("option:selected").text(), AddressLifePlan: $('#TextBoxCity').val() + ' ' + $('#TextBoxState').val() + ' ' + $('#TextBoxZipCode').val(),
        DateOfBirth: $("#TextBoxDateOfBirth").val(), AddressCCO: $('#TextBoxAddress2CCO').val() + ' ' + $('#DropDownCityCCO').val() + ' ' + $('#DropDownStateCCO').val() + ' ' + $('#TextBoxZipCodeCCO').val(), ReportedBy: reportedBy

    };

    $.ajax({
        type: "POST",
        data: data,
        url: GetAPIEndPoints("HANDLELIFEPLANVERSIONING"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {

                InsertDocumentPath(result);
            }
            else {
                showErrorMessage(result.Message);
            }

        },

        error: function (xhr) { HandleAPIError(xhr) }
    });
}
 function InsertDocumentPath(result) {
     var data = {
         ClientID: result.AllTab[0].ClientId, DocumentName: result.AllTab[0].Documentname, DocumentFileName: result.AllTab[0].Documentname, Comments: "test"
     };
     $.ajax({
         type: "POST",
         url: 'https://staging-api.cx360.net/api/Client/SaveClientDocument?' + jQuery.param(data),
         headers: {
             TOKEN: token,
         },
         contentType: "application/json; charset=utf-8",
         success: function (result) {
             if (!isEmpty(result) && result.MessageCode=="200") {
                 showRecordSaved(result.Message);
                 location.href = "lifeplan-management.html?ClientID=" + clientId;
             }
             else {
                 showErrorMessage(result.Message);
             }

         },

         error: function (xhr) { HandleAPIError(xhr) }
     });
}
function DownloadPDFLifePlan() {
    var data = {
        TabName: "LifePlanPDF", LifePlanId: $(".lifePlanId").val(), IndividualName: $("#DropDownClientId").find("option:selected").text(), AddressLifePlan: $('#TextBoxCity').val() + ' ' + $('#TextBoxState').val() + ' ' + $('#TextBoxZipCode').val(),
        DateOfBirth: $("#TextBoxDateOfBirth").val(), AddressCCO: $('#TextBoxAddress2CCO').val() + ' ' + $('#DropDownCityCCO').val() + ' ' + $('#DropDownStateCCO').val() + ' ' + $('#TextBoxZipCodeCCO').val()

    };

    fetch(GetAPIEndPoints("FILLABLELIFEPLANPDF"), {
        body: JSON.stringify(data),
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5"
        },
    })
        .then(response => response.blob())
        .then(response => {
            const blob = new Blob([response], { type: 'application/pdf' });
            const downloadUrl = URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = downloadUrl;
            a.download = "lifeplan_1_" + $(".lifePlanId").val() + "_" + getFormattedTime() + ".pdf";
            document.body.appendChild(a);
            a.click();
        })

}
function fillFacilityCode(object) {
    var selectedValue = $(object).val();
    var jsonObject = $("#DropDownFacilityName").attr("josn");
    var parse = jQuery.parseJSON(jsonObject);
    var res = $.grep(parse, function (FacilityName) {
        return FacilityName.UD_ExternalStaffAssignmentID == selectedValue;
    });
    if (res.length > 0) {
        $("#TextBoxFacilityCode").val(selectedValue);
        $(".section4 #TextBoxFirstName").val(res[0].FirstName)
        $(".section4 #TextBoxLastName").val(res[0].LastName);
    }

}
//#endregion

//#region get default lifeplan sections data
function GetSuggestedOutcomesStrategiesTabDetails(clientId) {

    var json = [],
        item = {},
        tag = "LifePlanId";
    // if ($("#TextBoxSuggestedSupportStrategieId").val() != "" || $("#TextBoxSuggestedSupportStrategieId").val() != null) {

    item[tag] = $("#TextBoxSuggestedSupportStrategieId").val();
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "GetSuggestedOutcomesSupportStrategies", ClientId: clientId, Json: JSON.stringify(json), ReportedBy: reportedBy },
        dataType: 'json',
        url: GetAPIEndPoints("SUGGESTEDOUTCOMESSTRATEGIES"),
        headers: { 'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5" },
        success: function (result) {


            if (result.Success == true) {

                var jsonStringyfy = JSON.stringify(result.suggestedOutcomesSupportStrategiesTab) == "null" ? "{}" : JSON.stringify(result.suggestedOutcomesSupportStrategiesTab);
                var table = $('#tblStrategiesOutcomesSupportStrategies').DataTable({
                    "stateSave": true,
                    "bDestroy": true,
                    "paging": true,
                    "searching": true,
                    'columnDefs': [
                        {
                            'targets': 0,
                            'checkboxes': {
                                'selectRow': true
                            },
                            "render": function (data, type, full, meta) {
                                return '<input type="checkbox" name="RadioSuggestedSupportStrategie" value="' + $('<div/>').text(meta.row).html() + '">';
                            }
                        }, {
                            'targets': 10,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).attr('class', 'hidden');
                            }
                        }, {
                            'targets': 11,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).attr('class', 'hidden');
                            }
                        }, {
                            'targets': 12,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).attr('class', 'hidden');
                            }
                        }, {
                            'targets': 13,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).attr('class', 'hidden');
                            }
                        }, {
                            'targets': 14,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).attr('class', 'hidden');
                            }
                        },
                        {
                            'targets': 15,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).attr('class', 'hidden');
                            }
                        }
                    ],
                    'select': {
                        'style': 'multi'
                    },
                    'order': [[1, 'asc']],


                    "aaData": JSON.parse(jsonStringyfy),
                    "columns": [{ "data": "CqlPomsGoal" }, { "data": "CqlPomsGoal" }, { "data": "CcoGoal" }, { "data": "ProviderAssignedGoal" }, { "data": "ProviderLocation" }, { "data": "ServicesType" }, { "data": "Frequency" },
                    { "data": "Quantity" }, { "data": "TimeFrame" }, { "data": "SpecialConsiderations" }, { "data": "ServicesTypeId" }, { "data": "FrequencyId" }, { "data": "QuantityId" }, { "data": "TimeFrameId" }, { "data": "CqlPomsGoalId" }, { "data": "ProviderLocationId" }]
                });
                jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');
                //table.columns([10, 11, 12, 13]).visible(false);
            }
            else {
                showErrorMessage(result.Message);
            }

        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
    //  }



}
function GetDefaultMeetingHistoryDetails(clientId) {
    var json = [],
        item = {},
        tag = "LifePlanId";
    item[tag] = $("#TextBoxNoteID").val();
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "GetNoteMeetingHistory", ClientId: clientId, Json: JSON.stringify(json), ReportedBy: reportedBy },
        dataType: 'json',
        url: GetAPIEndPoints("SUGGESTEDOUTCOMESSTRATEGIES"),
        headers: { 'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5" },
        success: function (result) {


            if (result.Success == true) {

                var jsonStringyfy = JSON.stringify(result.MeetingHistorySummaryTab) == "null" ? "{}" : JSON.stringify(result.MeetingHistorySummaryTab);
                var table = $('#tblMeetingHistoryExported').DataTable({
                    "stateSave": true,
                    "bDestroy": true,
                    "paging": true,
                    "searching": true,
                    'columnDefs': [
                        {
                            'targets': 0,
                            'checkboxes': {
                                'selectRow': true
                            },
                            "render": function (data, type, full, meta) {
                                return '<input type="checkbox" name="RadioMeetingHistory" value="' + $('<div/>').text(meta.row).html() + '">';
                            }
                        },
                        {
                            'targets': 5,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).attr('class', 'hidden');
                            }
                        },
                    ],
                    'select': {
                        'style': 'multi'
                    },
                    'order': [[1, 'asc']],

                    "aaData": JSON.parse(jsonStringyfy),
                    "columns": [{ "data": "NoteTypeText" },{ "data": "NoteTypeText" }, { "data": "EventDate" }, { "data": "Subject" }, { "data": "MeetingReason" },{ "data": "NoteType" }]
                });
                jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');
            }
            else {
                showErrorMessage(result.Message);
            }

        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}
function OpenDefaultOutComeModal(closeModal,openModal) {
    $("#" + closeModal).modal("hide");
    $("#" + openModal).modal("show");
}
//#endregion 

//#region lifeplan notifications
function InsertModifyLifeplanNotifications() {
    var json = [],
        item = {},
        tag;

    $('.section6 .form-control').each(function () {
        tag = $(this).attr('name').replace("TextArea", "").replace("TextBox", "").replace("DropDown", "").replace("Checkbox", "").replace("Radio", "");
        if ($(this).hasClass("req_feild")) {
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
            else {
                item[tag] = jsonWrapperWithTimePicker(tag, this);
            }
        }
    });
    item["DocumentVersionId"] = $("#TextBoxDocumentVersionId").val();
    item["LifePlanId"] = $("#TextBoxLifePlanId").val();
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "InsertLifePlanNotifications", Json: JSON.stringify(json), ReportedBy: reportedBy },
        url: GetAPIEndPoints("INSERTMODIFYLIFEPLANDETAIL"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                if ($("#TextBoxLifeplanNotifiactionId").val() == null || $("#TextBoxLifeplanNotifiactionId").val() == "") {

                    showRecordSaved("Record Saved.");

                }
                else {
                    showRecordSaved("Record Updated.");

                }
                cleartextLifePlanNotifications();
                FillLifePlanNotificationTable(result);
                $("#exampleModa6").modal("hide");
            }
            else {
                showErrorMessage(result.Message);
            }


        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}
function cleartextLifePlanNotifications() {
   
    $(".section6 #TextBoxNotificationDate").val("");
    $(".section6 select").val("");
    $(".section6 #DropDownNotificationProvider").select2(null).trigger('change');
    $(".section6 #DropDownNotificationContCircSup").select2(null).trigger('change');
    $(".section6 #DropDownNotificationReason").select2(null).trigger('change');
    $(".section6 #DropDownNotificationType").select2(null).trigger('change');
    $(".section6 #DropDownNotificationAccptAckwStatus").select2(null).trigger('change');
    $(".section6 #TextBoxNotificationComments").val("");
    $(".section6 #TextBoxLifePlanNotifiactionId").val("");
}
function FillLifePlanNotificationTable(result)
{
    result = result.AllTab[0].JSONData;
    $('#tblLifeplanNotifications').DataTable().clear().draw();
    $('#tblLifeplanNotifications').DataTable({
        "stateSave": true,
        "bDestroy": true,
        "paging": true,
        "searching": true,
        "autoWidth": false,
        "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
        "aaData": JSON.parse(result),
        "columns": [{ "data": "NotificationDate" }, { "data": "NotificationProvider" }, { "data": "NotificationContCircSup" }, { "data": "NotificationReason" }, { "data": "NotificationType" }, { "data": "NotificationAccptAckwStatus" }, { "data": "Actions" }]
    });
    jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');
}
function GetLifePlanTabDetails(){
    var json = [],
        item = {},
        tag = "LifePlanId";
    if ($("#TextBoxLifePlanId").val() != "") {
        item[tag] = $("#TextBoxLifePlanId").val();
        json.push(item);
        $.ajax({
            type: "POST",
            data: { TabName: "GetLifePlanTabDetails", LifePlanId: $("#TextBoxLifePlanId").val(), Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "select" },
            dataType: 'json',
            url: GetAPIEndPoints("HANDLELIFEPLANNOTIFICATIONS"),
            headers: {
                'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
            },
            success: function (result) {
                if (result.Success == true) {
                    FillLifePlanNotificationTable(result);
                }
                else {
                    showErrorMessage(reslut.Message);
                }

            },
            error: function (xhr) { HandleAPIError(xhr) }
        });
    }
}
function DeleteLifeplanNotifications(id) {
    var json = [],
        item = {},
        tag = "Id";

    item[tag] = id;
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "GetLifePlanNotifications", LifePlanId: "", Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "deleteById" },
        url: GetAPIEndPoints("HANDLELIFEPLANNOTIFICATIONS"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                FillLifePlanNotificationTable(result);
                showRecordSaved("Record Deleted.");
            }
            else {
                showErrorMessage(result.Message);
            }


        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}
function EditLifePlanNotifications(e, id) {

    var json = [],
        item = {},
        tag = "Id";

    item[tag] = id;
    json.push(item);
    $.ajax({
        type: "POST",
        data: { TabName: "GetLifePlanNotifications", LifePlanId: "", Json: JSON.stringify(json), ReportedBy: reportedBy, Mode: "selectById" },
        url: GetAPIEndPoints("HANDLELIFEPLANNOTIFICATIONS"),
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (result) {
            if (result.Success == true) {
                if (result.AllTab[0].JSONData.length > 0) {
                    result = JSON.parse(result.AllTab[0].JSONData);
                    $(".section6 #TextBoxNotificationDate").val(result[0].NotificationDate);
                    $(".section6 #DropDownNotificationProvider").select2('val', [result[0].NotificationProviderId == "" ? null : result[0].NotificationProviderId]);
                    $(".section6 #DropDownNotificationContCircSup").select2('val', [result[0].NotificationContCircSupId == "" ? null : result[0].NotificationContCircSupId]);
                    $(".section6 #DropDownNotificationReason").select2('val', [result[0].NotificationReasonId == "" ? null : result[0].NotificationReasonId]);
                    $(".section6 #DropDownNotificationType").select2('val', [result[0].NotificationTypeId == "" ? null : result[0].NotificationTypeId]);
                    $(".section6 #DropDownNotificationAccptAckwStatus").select2('val', [result[0].NotificationAccptAckwStatusId == "" ? null : result[0].NotificationAccptAckwStatusId]);
                    $(".section6 #TextBoxNotificationComments").val(result[0].NotificationComments);
                    $(".section6 #TextBoxLifePlanNotifiactionId").val(result[0].LifePlanNotifiactionId);
                }
                InActiveCircleOfSupport(result);
                $("#exampleModa6").modal("show");
            }
            else {
                showErrorMessage(result.Message);
            }

        },
        error: function (xhr) { HandleAPIError(xhr) }

    });
}
function InActiveCircleOfSupport(result) {
    if (result.length > 0 && (result[0].NotificationContCircSupId > 0 && result[0].NotificationContCircSup != "") && (isEmpty($(".section6 #DropDownNotificationContCircSup").val()))) {
        SetDropDownAndParentValue($("#DropDownNotificationContCircSup"), result[0].NotificationContCircSupId, result[0].NotificationContCircSup);
    }
}
function SetDropDownAndParentValue(DropDownObject, GlobalCodeId, CodeName) {
  
    var opt = document.createElement("Option");
    opt.textContent = RTrim(CodeName);
    opt.value = GlobalCodeId;
    opt.addClass = "inactive";
    DropDownObject[0].appendChild(opt);
    opt.selected = true;

   
}
function BindContactControlsDropDown(result) {
    $("#DropDownNotificationContCircSup").empty();

    $.each(result, function (data, value) {
        $("#DropDownNotificationContCircSup").append($("<option></option>").val(value.FundalNaturalCommunityResourcesId).html(value.LastName + " , " + value.FirstName + " , " + value.Role));
    });
    $("#DropDownNotificationContCircSup").prepend("<option value='' selected='selected'>--Select--</option>");
}
//#endregion
//#region submit and review approval
function OpenApproval(approval) {
    if (approval == "SubmitApproval") {
        var date = new Date();
        $('#TextBoxReviewDate').val(formatDate(date, 'MM/dd/yyyy'));
        $(".review-aprroval-status").hide();
        $(".submit-aprroval-submittedto").show();
        $(".submit-aprroval-status").show();
        $(".changelabeltxt").text("Submittion Message");
            $("#DropDownSubmitStatus option:contains(Submitted)").prop('selected', true);
            $("#DropDownSubmitStatus").prop('disabled', true);
     
        $("#btnSubmitReview").addClass("btnSubmit");
        $("#btnSubmitReview").removeClass("btnApproval");
    }
    else {
        var date = new Date();
        $('#TextBoxReviewDate').val(formatDate(date, 'MM/dd/yyyy'));
        $(".submit-aprroval-submittedto").hide();
        $(".submit-aprroval-status").hide();
        $(".review-aprroval-status").show();
        $(".changelabeltxt").text("Comments");
        $("#btnSubmitReview").addClass("btnApproval");
        $("#btnSubmitReview").removeClass("btnSubmit");

    }
    $("#submitReviewModal").modal("show");
}

function EpinSignatureValidation() {
  
    $('#TextBoxElectronicSignature').on("change", function () {
        if ($(this).val() != "") {
            $.ajax({
                type: "GET",
                data: { "EPIN": $(this).val() },
                url: Cx360URL+"/api/Client/ValidateEPIN",
                headers: {
                    'Token': token,
                },
                success: function (response) {
                    if (response.length > 0) {
                        var date = new Date();
                        $("#TextBoxStaffName").val(response[0].FirstName + " , " + response[0].LastName);
                        $("#TextBoxStaffTitle").val(response[0].Title);
                        $("#TextBoxElectronicSignature_SignedOn").val(formatDate(date, 'MM/dd/yyyy hh:mm a'));
                    }
                    else {
                        $("#TextBoxStaffName").val("");
                        $("#TextBoxStaffTitle").val("");
                        $("#TextBoxElectronicSignature_SignedOn").val("");
                    }
                },
                error: function (xhr) {
                    HandleAPIError(xhr); $('#TextBoxElectronicSignature').val("");
                    $("#TextBoxStaffName").val("");
                    $("#TextBoxStaffTitle").val("");
                    $("#TextBoxElectronicSignature_SignedOn").val("");
                }
                 
            });
        }
    });
}
function ValidateApprovalForm(elem) {
    if ($(elem).hasClass("btnSubmit")) {
        if ($("#DropDownSubmittedTo").val() == "-1" || $("#DropDownSubmittedTo").val() == "") {
            $("#DropDownSubmittedTo").siblings("span.errorMessage").removeClass("hidden");
            $("#DropDownSubmittedTo").focus();
            return false;
        }
        else if ($("#DropDownSubmitStatus").val() == "-1" || $("#DropDownSubmitStatus").val() == "") {
            $("#DropDownSubmitStatus").siblings("span.errorMessage").removeClass("hidden");
            $("#DropDownSubmitStatus").focus();
            return false;
        }
        else if ($("#TextBoxElectronicSignature").val() == "") {
            $("#TextBoxElectronicSignature").siblings("span.errorMessage").removeClass("hidden");
            $("#TextBoxElectronicSignature").focus();
            return false;
        }
       else if ($("#TextBoxReviewDate").val() == "") {
            $("#TextBoxReviewDate").siblings("span.errorMessage").removeClass("hidden");
            $("#TextBoxReviewDate").focus();
            return false;
        }
    }
    else {
        if ($("#TextBoxReviewDate").val() == "") {
            $("#TextBoxReviewDate").siblings("span.errorMessage").removeClass("hidden");
            $("#TextBoxReviewDate").focus();
            return false;
        }
       else if ($("#DropDownReviewStatus").val() == "-1" || $("#DropDownReviewStatus").val() == "") {
            $("#DropDownReviewStatus").siblings("span.errorMessage").removeClass("hidden");
            $("#DropDownReviewStatus").focus()
            return false;
        }
        else if ($("#TextBoxElectronicSignature").val() == "") {
            $("#TextBoxElectronicSignature").siblings("span.errorMessage").removeClass("hidden");
            $("#TextBoxElectronicSignature").focus();
            return false;
        }
    }
    return true;
}
function InsertModifySubmitReviewNotification(elem) {
    var data = "";
    if ($(elem).hasClass("btnSubmit")) {
        if (!ValidateApprovalForm(elem)) return;
        approvalStatus = $("#DropDownSubmitStatus option:selected").text();
        data = {
            UD_SubmissionDecisionFormID: "", FormName: "Life Plan", ClientID: clientId, KeyFieldID: $("#TextBoxLifePlanId").val(), Status: $("#DropDownSubmitStatus").val() ,
            SubmissionMessage: $("#TextBoxSubmissionMessage").val(), TabName: "SubmitApproval", SubmittedTo: $("#DropDownSubmitStatus").val(),
            ElectronicSignature: $("#TextBoxElectronicSignature").val(), ElectronicSignature_SignedOn: $("#TextBoxElectronicSignature_SignedOn").val(),
            SubmittedOn: $("#TextBoxReviewDate").val(), StaffTitle: $("#TextBoxStaffTitle").val(), StaffName: $("#TextBoxStaffName").val(), ReportedBy:reportedBy
        }
    }
    else {
        if (!ValidateApprovalForm(elem)) return;
        approvalStatus = $("#DropDownReviewStatus option:selected").text();
        data = {
            UD_SubmissionDecisionFormID: "", FormName: "Life Plan", ClientID: clientId, KeyFieldID: $("#TextBoxLifePlanId").val(), Status: $("#DropDownReviewStatus").val(),
            SubmissionMessage: $("#TextBoxSubmissionMessage").val(),TabName: "ReviewApproval", SubmittedTo: "",
            ElectronicSignature: $("#TextBoxElectronicSignature").val(), ElectronicSignature_SignedOn: $("#TextBoxElectronicSignature_SignedOn").val(),
            SubmittedOn: $("#TextBoxReviewDate").val(), StaffTitle: $("#TextBoxStaffTitle").val(), StaffName: $("#TextBoxStaffName").val(), ReportedBy: reportedBy
        }
    }
    $.ajax({
        type: "POST",
        url: GetAPIEndPoints("INERTMODIFYSUBMISSIONFORM"),
        data: data,
        headers: {
            'Token': "6C194C7A-A3D0-4090-9B62-9EBAAA3848C5",
        },
        success: function (response) {
            if (response.Success == true) {
                showRecordSaved("show record saved");
                $("#submitReviewModal").modal("hide");
                clearsubmitReviewModal();
                ShowHideSubmitReviewButton(JSON.parse(response.AllTab[0].JSONData)[0].Status);
               
            }
            else {
                showErrorMessage(response.Message);
            }
        },
        error: function (xhr) { HandleAPIError(xhr)}

    });
}
function clearsubmitReviewModal() {
    $(".submitReviewModal select").val("");
    $(".submitReviewModal #DropDownSubmittedTo").select2(null).trigger('change');
    $(".submitReviewModal input").val("");
    $(".submitReviewModal textarea").val("");
    $(".submitReviewModal #btnSubmitReview").removeClass("btnApproval");
    $(".submitReviewModal #btnSubmitReview").removeClass("btnSubmit");
}
//#endregion 
function AdjustPaddingResize() {
    if (Number(actualZoom) <= 1) {
        $(".screenZoom").addClass("setNormalZoomPading").removeClass("setMaxZoomPading");
    } else {
        $(".screenZoom").addClass("setMaxZoomPading").removeClass("setNormalZoomPading");
    }
}


function BindUserDefinedCodes(DropDown, Category,_age) {
    $.ajax({
        type: "GET",
        url: Cx360URL + '/api/Incident/GetUserDefinedOptionByCategory',
        data: { 'CateegoryName': Category, },
        headers: {
            'TOKEN': token
        },
        success: function (result) {
          // BindDropDownOptions(result, DropDown, "UDID", "UDDescription");
          var val= "UDID", options="UDDescription";
            if (Category == 'ValuedOutcomes_CQLPOMSGoal' && getAge(_age) >= 18) {
                $.each(result, function (data, value) {
                    $(DropDown).append($("<option></option>").val(value[val]).html(value[options]));
                });
            }
            else{
                $.each(result, function (data, value) {
                    $(DropDown).append($("<option></option>").val(value[val]).html(value[options].replace('People', 'Children and their families')));
                });
                }
        },
        error: function (xhr) { HandleAPIError(xhr) }
    });
}