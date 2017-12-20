# tjx-script
It's that time of the year. This console app will help you modify TJX Playlist JSONs.

## Usage
- build the app. 
- run it passing the file you want to modify.

## Algorithm
- find the playlist with the most recent StartDateTime which starts earlier than January 2018 (as per Morgan's request).
- make a copy of said playlist.
- in that copy, only allow songs that:
  - have a start date >= 26th of December.
  - have a tag that is not "holiday" or "xmas" (case insensitive).
- update the first item in the sequence to have a StartDateTime of 2017-12-26T00:00:00.
- append this copy to the original file, with a CreatedDate of 13th of December.
