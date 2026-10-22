function SubmitForm(formId = 'MyForm') {
    document.getElementById(formId).submit();
}

function CreateOptionTag(item, selectedValue) {
    var option = document.createElement("option");

    option.value = item.value;
    option.textContent = item.text;
    if (selectedValue == option.value)
        option.selected = 'selected';

    return option;
}

function LoadProvinces(selectedProvinceId = 0, provinceSelectTagId = 'province') {
    var provinceSelect = document.getElementById(provinceSelectTagId);

    fetch("/provinces")
        .then(function (response) {
            return response.json();
        })
        .then(function (data) {
            if (selectedProvinceId == 0) {
                provinceSelect.appendChild(CreateOptionTag({ value: "", text: "لطفا یک مورد را انتخاب نمایید" }, 0));
            }
            data.forEach(function (item) {
                provinceSelect.appendChild(CreateOptionTag(item, selectedProvinceId));
            });
        });
}
function PopulateSelect(jsonString, selectId, selectedValue) {
    var select = document.getElementById(selectId);

    select.innerHTML = "";

    if (selectedValue === null)
        select.appendChild(CreateOptionTag({ value: "", text: "لطفا یک مورد را انتخاب نمایید" }, 0));

    var jsonData = JSON.parse(jsonString);

    for (var i = 0; i < jsonData.length; i++) {
        select.appendChild(CreateOptionTag({ value: jsonData[i].value, text: jsonData[i].text }, selectedValue));
    }
}

function formatNumber(input, hasFloatingPoint = false) {
    // Remove any non-digit and non-dot characters
    input.value = input.value.replace(/[^0-9.]/g, '');

    // If input has a floating point
    if (hasFloatingPoint) {
        // Allow only one dot and prevent dot being the first character
        if (input.value.indexOf('.') === 0) {
            input.value = '';
        } else {
            input.value = input.value.replace(/(\..*)\./g, '$1');
        }

        // Allow only two decimal places
        const decimalIndex = input.value.indexOf('.');
        if (decimalIndex !== -1 && input.value.length - decimalIndex > 3) {
            input.value = input.value.slice(0, decimalIndex + 3);
        }
    } else {
        // Prevent dot from being entered
        input.value = input.value.replace('.', '');
    }

    // Format the input number by separating it into groups of three digits
    input.value = input.value.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}