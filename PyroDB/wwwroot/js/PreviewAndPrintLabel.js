//----------------------------------------------------------------------------
//
//  $Id: PreviewAndPrintLabel.js 11419 2010-04-07 21:18:22Z vbuzuev $ 
//
// Project -------------------------------------------------------------------
//
//  DYMO Label Framework
//
// Content -------------------------------------------------------------------
//
//  DYMO Label Framework JavaScript Library Samples: Preview and Print label
//
//----------------------------------------------------------------------------
//
//  Copyright (c), 2010, Sanford, L.P. All Rights Reserved.
//
//----------------------------------------------------------------------------


(function () {
    // stores loaded label info
    var label;

    // called when the document completly loaded
    function onload() {
        var labelData = document.getElementById('labelData');
        var printersSelect = document.getElementById('printersSelect');
        var printButton = document.getElementById('printButton');

        // initialize controls
        printButton.disabled = true;

        // Generates label preview and updates corresponend <img> element
        // Note: this does not work in IE 6 & 7 because they don't support data urls
        // if you want previews in IE 6 & 7 you have to do it on the server side
        function updatePreview()
        {
            if (!label)
                return;

            var pngData = label.render();

            var labelImage = document.getElementById('labelImage');
            labelImage.src = "data:image/png;base64," + pngData;
        }

        // loads all supported printers into a combo box 
        function loadPrinters()
        {
            var printers = dymo.label.framework.getPrinters();
            if (printers.length == 0) {
                alert("No DYMO printers are installed. Install DYMO printers.");
                return;
            }

            for (var i = 0; i < printers.length; i++) {
                var printer = printers[i];
                if (printer.printerType == "LabelWriterPrinter") {
                    var printerName = printer.name;

                    var option = document.createElement('option');
                    option.value = printerName;
                    option.appendChild(document.createTextNode(printerName));
                    printersSelect.appendChild(option);
                }
            }
        }

        // prints the label
        printButton.onclick = function ()
        {
            try {
                if (!label) {
                    alert("Load label before printing");
                    return;
                }

                //alert(printersSelect.value);
                label.print(printersSelect.value);
                //label.print("unknown printer");
            }
            catch (e) {
                alert(e.message || e);
            }
        }

        labelData.onchange = function () {
            loadLabelFromWeb();
        }

        function getAddressLabelXml()
        {
            var labelXml = atob(labelData.value);
            return labelXml;
        }

        function loadLabelFromWeb()
        {
            label = dymo.label.framework.openLabelXml(getAddressLabelXml());

            $.each(proptable, function (key, value)
            {
                try {
                    label.setObjectText(key, value);
                }
                catch (e) {
                }

            });

            updatePreview();
            printButton.disabled = false;
        }

        loadLabelFromWeb();
        loadPrinters();
    };

    function initTests()
	{
		if(dymo.label.framework.init)
		{
			//dymo.label.framework.trace = true;
			dymo.label.framework.init(onload);
		} else {
			onload();
		}
	}

	// register onload event
	if (window.addEventListener)
		window.addEventListener("load", initTests, false);
	else if (window.attachEvent)
		window.attachEvent("onload", initTests);
	else
		window.onload = initTests;

}());