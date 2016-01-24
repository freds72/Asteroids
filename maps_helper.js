var http = require('http');
var fs = require('fs');
var async   = require('async');

// GPS to lat/long
// http://www.coordonnees-gps.fr/conversion-coordonnees-gps
var TILE_SIZE = 256;
function getLatLongInfo(latLng, zoom) {
    var scale = 1 << zoom;

    var worldCoordinate = project(latLng);

    var pixelCoordinate = {
        x: Math.floor(worldCoordinate.x * scale),
        y: Math.floor(worldCoordinate.y * scale)
    };

    var tileCoordinate = {
        x: Math.floor(worldCoordinate.x * scale / TILE_SIZE),
        y: Math.floor(worldCoordinate.y * scale / TILE_SIZE)
    };

    console.log("pixel:" + JSON.stringify(pixelCoordinate));
    console.log("tiles:" + JSON.stringify(tileCoordinate));

    return tileCoordinate;
}

// The mapping between latitude, longitude and pixels is defined by the web
// mercator projection.
function project(latLng) {
    var siny = Math.sin(latLng.lat * Math.PI / 180);

    // Truncating to 0.9999 effectively limits latitude to 89.189. This is
    // about a third of a tile past the edge of the world tile.
    siny = Math.min(Math.max(siny, -0.9999), 0.9999);

    return {
        x: TILE_SIZE * (0.5 + latLng.lng / 360),
        y: TILE_SIZE * (0.5 - Math.log((1 + siny) / (1 - siny)) / (4 * Math.PI))
    };
}

// JFK
getLatLongInfo({ lat: 40.6413111, lng: -73.77813909999998 }, 11);
	
// NEVADA top left
var tl = getLatLongInfo({ lat: 42.16340342422401, lng: -124.969482421875 }, 11);
// NEVADA bottom right
var br = getLatLongInfo({ lat: 34.92197103616377, lng: -113.04931640625 }, 11);
var count = (br.x - tl.x) * (br.y - tl.y);
console.log("tiles#:" + count);

var tiles = [];
for(var i = tl.x;i<=br.x;i++)
{
    for(var j=tl.y;j<=br.y;j++)
    {
        tiles.push({x:i,y:j});
    }
}

async.forEachOf(tiles, 
    function(tile, callback) {
        var file = fs.createWriteStream('C:/Users/Frederic/Downloads/maps/' + tile.y + '.' + tile.x +'.png');
        var request = http.get(
            'http://d1pc149g092dlb.cloudfront.net/Maps/Tiles/Sectional/Z11/' + tile.y + '/' + tile.y +'.png', 
            function(response) {
                console.log("got: " + tile.x + "/" + tile.y);
                response.pipe(file);
            });
    },
    function(err){
        console.log(err);
    });


/*
$client = new-object System.Net.WebClient;
for($i=313 ; $i -le 380 ;$i++) {
    for($j=758 ; $j -le 811 ;$j++) {
        $client.DownloadFile( "http://d1pc149g092dlb.cloudfront.net/Maps/Tiles/Sectional/Z11/$j/$i.png", "C:\Users\Frederic\Downloads\map\$j.$i.png" );
     }
}
