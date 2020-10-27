<?php
require_once 'functions.php';

$g = $_GET;
if(isset($g['all'])) GetAllEvents();
else if(!empty($g['page'])) 
{
    if(intval($g['page']) == 0) show_error(400, "The parameter 'page' should be an integer and shouldn't be 0 (zero).");
    GetEvents($g['page']-1);
}
else if(!empty($g['last']) && $g['last'] > 0 )
{
    $last = true;
    $lastcount = $g['last'];
    GetAllEvents();
}
else if(isset($g['last']))
{
    $last = true;
    $lastcount = 1;
    GetAllEvents();
}
else
{
    show_error(405, "Method not allowed, see (https://api.berkealp.net/kandilli.html)");
}