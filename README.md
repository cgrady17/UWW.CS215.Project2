# UWW.CS215.Project2
Simple RSA encryption/decryption example

Follows the following examples

1. Choose values for `p` and `q` of type `Int64`, using the largest possible values
2. Calculate `n` equivalent to `p * q`
3. Calculate the `Φ(n)` (phi of `n`) equivalent to `(p-1) * (q-1)`
4. Choose a value for `e` such that `1 < e < Φ(n)` and `e` and `Φ(n)` are coprime
5. Calculate `d` such that `(d * e) % Φ(n) = 1`
6. Set a Public Key equivalent to `(e,n)`
7. Set a Private Key equivalent to `(d,n)`
8. Encrypt `m` such that `c = m^e % n`
9. Decrypt `m` such that `m = c^d % n`

The will output the values of the Public Key, Private Key, `m` and `c`.
