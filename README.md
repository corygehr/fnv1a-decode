# fnv1a-decode
FNV1A Hash Resolver. Created during a six-hour CTF.

## Acknowledgements

I took the hashing algorithm from [jslicer](https://github.com/jslicer/FNV-1a/) since I needed a quick solution and hadn't used FNV1A. I only kept the 128-bit portion of the code since the CTF only required that size.

# Overview

You **cannot** decrypt a hash. However, you **can** hash text and see if the resulting hash matches a hash. If you find a match, well... you know its cleartext value. It's not perfect and certainly not fast, but at a high level this is how attackers crack encoded strings. This process is also how websites authenticate users without storing a user's password in cleartext (though I'm way, way oversimplifying that process).

This application specifically targets the FNV1A algorithm, using a 128 bit length. With most hashing algorithms I would rely on a program like [hashcat](https://hashcat.net/hashcat/) which is well-optimized for multiple platforms and hashing using a computer's graphics card (which are much better than CPUs at hashing). However, hashcat does not support this algorithm so during a time-contrained competition, I needed to build my own application quickly to process a collection of hashes to score points. The algorithm and bit length were already provided.

## Procedure

Brute-force is one option given lots of time. In this scenario time was limited. Thus, the program accepts a wordlist from which it computes hashes using the FNV1A-128 algorithm.

1. Takes two input files (txt).
  * WordList
  * Hashes to encrypt
2. With each plaintext string in the wordlist, generate its FNV1A-128 hash.
3. Compare the generated hash against the list of provided hashes.
  * If there's a match, log the plaintext string.
  * In this scenario, the output is expected in {original_hash}: {plaintext} format and must stay in the same order as presented from the input hash file. However, the upload service allowed submitted several different files of cracked hashes, as long as they remained in the same order as originally provided.

Actual input:

fnv1a.exe {path_to_hashes.txt} {path_to_wordlist.txt} {path_to_output.txt} ({path_to_leftover_hashes.txt})

The last parameter is optional. In the competition it was known that the rockyou wordlist would resolve at least half of the hashes in the list while the others were random strings. Thus, multiple rounds of running this application were necessary with different wordlists. The last parameter was helpful because it allows just re-using the leftover hash filename as the input hash file.

# How to improve?

* Multi-threading
  * Loading the target hashes first, then sending them to multiple threads which evenly distribute the wordlist entries and process results would be smarter than the current model which is a single, synchronous process. For the majority of the hashes the super-inefficient mechanism worked well, but as the amount of words to try increases with each bit of entropy, performance dropped significantly.
* Dump hashes as found
  * This would help if there's a crash, or if the competition is about to end and you just want to submit what you have. This requires a bit of extra file manipulation to ensure the order is preserved but worth the effort.
* Remove a hash from consideration after it has been matched.
  * Removes another string from the match check which can slightly optimize performance.

A first iteration of this program pre-computed the hashes of the wordlist and afterward searching for matches in the file. However, this can eat lots of memory if wordlists grow (e.g. all combinations of lowercase letters using 8-character strings grows to hundreds of gigabytes quickly - per crunch). Thus that model was abandoned after the input wordlist grew too quickly.

### Why C# though?

I work with it every day. If I need to do something quickly, I use C#. If I had a lot more time I would probably have tried to do this in another language to brush up on my skills.
