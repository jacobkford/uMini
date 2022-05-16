var cookieName = "TimeZoneOffset";
var currentOffset = Intl.DateTimeFormat().resolvedOptions().timeZone;

if (!$.cookie(cookieName) || $.cookie(cookieName) !== currentOffset) {
    setCookie(cookieName, currentOffset);
}

function setCookie(name, value) {
    $.cookie(name, value);
    location.reload();
}