// JScript File

//*******************************************************************
//	Function Name	:	tableToCSV
//	Input			:	None
//	Description		:	export table string To CSV
//*******************************************************************
function tableToCSV(e,t){

if (navigator.userAgent.indexOf('MSIE') != -1 )
 {
var uri = 'data:application/csv;charset=UTF-8,' + encodeURIComponent(e);
var filename=t + ".csv";
var w = window.open();
var doc = w.document;
doc.open( uri);    
doc.write(e);
doc.close();
var success=doc.execCommand("SaveAs", null, filename);
if(success)
{
w.close();
}
}
else
{
blob = new Blob([e], { type: 'text/csv' }); //new way
var csvUrl = URL.createObjectURL(blob);
var n=document.createElement('a');
n.setAttribute("href",csvUrl);
n.setAttribute("download",t+".csv");
document.body.appendChild(n);
n.click()
}
}
//*******************************************************************
//	Function Name	:	CSVCell
//	Input			:	None
//	Description		:	Add CSV Cell
//*******************************************************************
function CSVCell(sText, bColSep, addQuote) {
    var ret = "";
    var sInfo = "";
    
    if(bColSep == "" || bColSep == null || bColSep == undefined) {bColSep = false};
    if(addQuote == "" || addQuote == null || addQuote == undefined) {addQuote = false};
    
    sText = sText.replace("<br>", ",");
    sText = sText.replace("<BR>", ",");
    
    if(addQuote) {
        sInfo = String.fromCharCode(34) + sText + String.fromCharCode(34);
    } else {
        sInfo = sText;
    }
    
    ret = sInfo;
    if(bColSep){
        ret = ret + ",";
    }
    
    return ret;
}

//*******************************************************************
//	Function Name	:	CSVNewLine
//	Input			:	None
//	Description		:	Add CSV New Line
//*******************************************************************
function CSVNewLine() {
    var ret = "";
    ret = ret + "\r\n";
    return ret;
}
