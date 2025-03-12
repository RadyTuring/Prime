const toggler = document.querySelector(".btn");
toggler.addEventListener("click",function(){
    document.querySelector("#sidebar").classList.toggle("collapsed");
});
$(document).ready(function() {
    $('#search-select').select2();
    $('#multi-search-select').select2();
});

$(document).ready(function() {
    $('#collapseSection').on('show.bs.collapse', function () {
        $('#toggleIcon').removeClass('fa-chevron-down').addClass('fa-chevron-up');
    }).on('hide.bs.collapse', function () {
        $('#toggleIcon').removeClass('fa-chevron-up').addClass('fa-chevron-down');
    });
});
new DataTable('#_datatable', {
    layout: {
        topStart: {
            buttons: ['pageLength', 'pdf', 'excel', 'print', 'copy', 'colvis', 'colvisRestore'],
        },

    }
});

