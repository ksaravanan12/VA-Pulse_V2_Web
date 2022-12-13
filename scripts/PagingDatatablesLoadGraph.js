// JScript File
//"sDom": 'f<"bottom"><"clear">'
function DatatablesLoadGraph(Id, nonSortingAry, DefaultSortCol, DefaultSortOrder, DisplayLength) {
   jQuery.noConflict();
   var oTable = $(Id).dataTable( {
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "aaSorting": [[ DefaultSortCol, DefaultSortOrder ]],
                    "iDisplayLength": -1,
                     "aLengthMenu": [
                         [-1,10, 50,100],
                         ["All",10, 50,100]
                     ]
                });
}

function DatatablesLoadGraph_New(Id, nonSortingAry, DefaultSortCol, DefaultSortOrder, DisplayLength) {
    jQuery.noConflict();
    var oTable = $(Id).dataTable({
        "bJQueryUI": true,
        "sPaginationType": "full_numbers",
        "aaSorting": [[DefaultSortCol, DefaultSortOrder]],
        "iDisplayLength": DisplayLength,
        "aLengthMenu": [
                         [-1, 10, 50, 100],
                         ["All", 10, 50, 100]
                     ]
    });
}

