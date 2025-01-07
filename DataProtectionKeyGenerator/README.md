# Generate Keys

```sh
New-SelfSignedCertificate -DnsName "VerifyDataProtection" -CertStoreLocation "cert:\LocalMachine\My"

$cert = Get-ChildItem -Path cert:\LocalMachine\My\BA8ED9E771D73B19E98B3C5ACD8997D4A61B8E8D

Export-Certificate -Cert $cert -FilePath ./Certificate.cer
```
