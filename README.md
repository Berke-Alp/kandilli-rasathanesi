# Kandilli Rasathanesi

Boğaziçi Üniversitesi Kandilli Rasathanesi'nde bulunan deprem verilerini çekmeye yarayan uygulama.

## PHP

PHP ile yazılmış olan versiyonda 4 adet seçenek bulunur:
- **?all** Rasathanede bulunan tüm verileri JSON listesi olarak çeker.
- **?page=x** Rasathanede bulunan **x** numaralı sayfadaki deprem verilerini JSON listesi olarak çeker.
- **?last** Rasathanede bulunan son deprem verisini JSON listesi olarak çeker.
- **?last=x** Rasathanede bulunan son **x** deprem verisini JSON listesi olarak çeker.

## C#

C# ile yazılmış olan versiyonda 4 adet seçenek bulunur. Bu seçenekleri programa argüman olarak gönderirsiniz.

Örnek: `> kandilli.exe all`

- **all** Rasathanede bulunan tüm verileri JSON listesi olarak çeker.
- **page=x** Rasathanede bulunan **x** numaralı sayfadaki deprem verilerini JSON listesi olarak çeker.
- **last** Rasathanede bulunan son deprem verisini JSON listesi olarak çeker.
- **last=x** Rasathanede bulunan son **x** deprem verisini JSON listesi olarak çeker.

C# versiyonunda çekilen veriler **events.json** isimli dosyaya kaydedilir.