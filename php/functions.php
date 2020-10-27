<?php

$apiroot = "http://sc3.koeri.boun.edu.tr/eqevents/";
$events = array();
$last = false;
$lastcount = 0;

function show_error($code, $message)
{
    $error = array(
        'message' => $message,
        'status' => $code
    );
    header("Content-Type: application/json");
    http_response_code($code);
    echo json_encode($error);
    die();
}

function GetPage($page)
{
    global $apiroot;

    if ($page < 0) return null;
    $url = $page == 0 ? $apiroot . "events.html" : $apiroot . "events" . $page . ".html";

    $ch = curl_init(); 
    curl_setopt($ch,CURLOPT_URL, $url); 
    curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
    $response = curl_exec($ch);
    $status = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    if($status == "404") $response = null;
    curl_close($ch);

    return $response;
}

function GetAllEvents()
{
    global $events, $last, $lastcount;

    libxml_use_internal_errors(true);

    $page = 0;
    $resp = GetPage($page);

    while(!empty($resp))
    {
        $dom = new DomDocument;
        $dom->loadHTML($resp);

        $xpath = new DomXPath($dom);
        $nodes = $xpath->query("//table[@class='index']/tr[position()>1]");
        foreach ($nodes as $i => $node) {
            // <tr> taglerinin içindeki tüm <td> elementleri
            $tds = $node->childNodes;

            // ID Numarasını kırpma işlemi
            $onclick = $node->attributes->getNamedItem("onclick")->nodeValue;
            $subone = substr($onclick, strpos($onclick, "/")+1);
            $id = substr($subone, 0, strpos($subone, "/"));

            // Event dictionary'si oluşturma işlemi
            $event = array(
                'ID' => $id,
                'MapImage' => "http://sc3.koeri.boun.edu.tr/eqevents/event/" . $id . "/" . $id . "-map.jpeg",
                'Time' => $tds[1]->nodeValue,
                'Magnitude' => floatval($tds[2]->nodeValue),
                'MagnitudeType' => $tds[3]->nodeValue,
                'Latitude' => str_replace("°", "&deg;", $tds[4]->nodeValue),
                'Longitude' => str_replace("°", "&deg;", $tds[5]->nodeValue),
                'Depth' => floatval($tds[6]->nodeValue),
                'Region' => $tds[7]->nodeValue,
                'AM' => $tds[8]->nodeValue,
                'LastUpdate' => $tds[9]->nodeValue
            );
            array_push($events, $event);
            if ($last && count($events) == $lastcount) break;
        }
        if ($last && count($events) == $lastcount) break;

        $page++;
        $resp = GetPage($page);
    }

    header("Content-Type: application/json");
    echo json_encode($events);
    exit;
}

function GetEvents($page)
{
    global $events;

    libxml_use_internal_errors(true);

    $resp = GetPage($page);

    if(!empty($resp))
    {
        $dom = new DomDocument;
        $dom->loadHTML($resp);

        $xpath = new DomXPath($dom);
        $nodes = $xpath->query("//table[@class='index']/tr[position()>1]");
        foreach ($nodes as $i => $node) {
            // TODO: Deprem ID numarası buradan alınacak
            // echo $node->attributes->getNamedItem("onclick")->nodeValue;

            // <tr> taglerinin içindeki tüm <td> elementleri
            $tds = $node->childNodes;

            $onclick = $node->attributes->getNamedItem("onclick")->nodeValue;
            $subone = substr($onclick, strpos($onclick, "/")+1);
            $id = substr($subone, 0, strpos($subone, "/"));

            $event = array(
                'ID' => $id,
                'MapImage' => "http://sc3.koeri.boun.edu.tr/eqevents/event/" . $id . "/" . $id . "-map.jpeg",
                'Time' => $tds[1]->nodeValue,
                'Magnitude' => floatval($tds[2]->nodeValue),
                'MagnitudeType' => $tds[3]->nodeValue,
                'Latitude' => str_replace("°", "&deg;", $tds[4]->nodeValue),
                'Longitude' => str_replace("°", "&deg;", $tds[5]->nodeValue),
                'Depth' => floatval($tds[6]->nodeValue),
                'Region' => $tds[7]->nodeValue,
                'AM' => $tds[8]->nodeValue,
                'LastUpdate' => $tds[9]->nodeValue
            );
            array_push($events, $event);
        }
    }

    header("Content-Type: application/json");
    echo json_encode($events);
    exit;
}