var cl;
$(document).ready(function () {
    cl = jQuery.fn.jColumnListView({
        id: 'cl1',
        width: 1200,
        columnWidth: 300,
        columnHeight: 300,
        columnMargin: 8,
        paramName: 'columnview',
        columnNum: 3,
        appendToId: 't1',
        elementId: 'scopes',
        removeAfter: false,
        columnMinWidth: 120,
        columnMaxWidth: 300,
        childIndicator: true,
        childIndicatorTextFormat: '%cvl-count%',
        leafMode: true,
        onItemChecked: function (ci) { console.log(ci); },
        onItemUnchecked: function (ci) { console.log(ci); },
        checkAllChildren: true,
    });
});