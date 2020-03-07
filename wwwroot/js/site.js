function loadTemplate() {
    var t = document.getElementById("template");
    var s = new Object();
    s = t.options[t.selectedIndex].value;
    var url = '/Templates/Load';
    $.ajax({
        type: 'post',
        url: url,
        data: { templateName: s },
        success: function (response) {
            var element = document.createElement('div');
            $(element).html(response).appendTo($("#UserContent"))
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    })

};

function castToObject(element) {
    var jsN = "{\"MainHeader\":null,\"SubHeader\":null,\"ShortDescription\":null,\"Description\":null,\"ImageUrl\":null,\"DateCreated\":null,\"DateModified\":null,\"Status\":\"null\",\"RegularPrice\":0,\"SalePrice\":0,\"IsShippingRequired\":false,\"Sku\":null,\"Slug\":\"null\",\"__RequestVerificationToken\":null}"
    var obj = JSON.parse(jsN);

    obj.MainHeader = element.elements.namedItem("MainHeader") === null ? null : element.elements.namedItem("MainHeader").value;
    obj.SubHeader = element.elements.namedItem("SubHeader") === null ? null : element.elements.namedItem("SubHeader").value;
    obj.ShortDescription = element.elements.namedItem("ShortDescription") === null ? null : element.elements.namedItem("ShortDescription").value;
    obj.Description = element.elements.namedItem("Description") === null ? null : element.elements.namedItem("Description").value;
    obj.Status = element.elements.namedItem("Status") === null ? null : element.elements.namedItem("Status").value;
    obj.RegularPrice = element.elements.namedItem("RegularPrice") === null ? 0 : element.elements.namedItem("RegularPrice").value;
    obj.SalePrice = element.elements.namedItem("SalePrice") === null ? 0 : element.elements.namedItem("SalePrice").value;
    obj.IsShippingRequired = element.elements.namedItem("IsShippingRequired") === null ? false : element.elements.namedItem("IsShippingRequired").value;
    obj.Sku = element.elements.namedItem("Sku") === null ? null : element.elements.namedItem("Sku").value;
    obj.Slug = element.elements.namedItem("Slug") === null ? null : element.elements.namedItem("Slug").value;
    obj.DateCreated = "0001-01-01T00:00:00";
    obj.DateModified = "0001-01-01T00:00:00";
    if (element.elements.namedItem("ImageUrl") !== null) {
        var tmp = [];
        for (var j = 0; j < element.elements.namedItem("ImageUrl").length; j++) {
            tmp.push(element.elements.namedItem("ImageUrl")[j].value);
        }
        obj.ImageUrl = tmp;
    }
    return obj;
}

function collectObjects() {
    var formsCollection = document.getElementsByTagName("form");
    var json = "[ ";
    for (i = 0; i < formsCollection.length; i++) {
        if (formsCollection[i].name !== "") {
            json += JSON.stringify(castToObject(formsCollection[i]))+",";
        }
    }
    json = json.replace(/,\s*$/, "") +"]";

    var url = '/Templates/Create';
    $.ajax({
        type: "post",
        url: url,
        data: { data: json },
        success: function (result) {
            window.location.href = result.url;
        },
        failure: function (response) {
            alert("Somthing went wrong! " + response.d);
        }
    })
};

$('#UserContent').on('click', 'a.remove_block', function (events) {
    $(this).parents('div').eq(1).remove();
});