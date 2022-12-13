// JScript File
//"sDom": 'f<"bottom"><"clear">'
function DatatablesLoadGraph(Id,nonSortingAry,DefaultSortCol,DefaultSortOrder,DisplayLength)
{  
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


// JScript File
//"sDom": 'f<"bottom"><"clear">'
function DatatablesLoadGraph_NoSort(Id,nonSortingAry,DefaultSortCol,DefaultSortOrder,DisplayLength)
{  
   jQuery.noConflict();
   var oTable = $(Id).dataTable( {
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",                   
                    "iDisplayLength": -1,
                    "ordering": false,
                    "searching": false,
                    "bInfo" : false,
                    "aLengthMenu": [
                         [-1,10, 50,100],
                         ["All",10, 50,100]
                     ]
                });
}
