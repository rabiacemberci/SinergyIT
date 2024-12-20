# Saga Pattern: Dağıtık Sistemlerdeki Problemleri Çözme Yaklaşımı

## Saga Pattern'in Amaçları

Saga pattern, mikroservis mimarisinde şu temel sorunları çözmeye odaklanır:

- **Dağıtık İşlem Koordinasyonu:** Birden fazla servis arasındaki işlem sırasını düzenler.
- **Veri Tutarlılığı:** ACID yerine eventual consistency sağlar, başarısız işlemleri telafi eder.
- **Hata Yönetimi:** Başarısızlık durumunda rollback veya kompanzasyon mekanizmaları ile sorunları giderir.
- **Merkezi Olmayan Kontrol:** Merkezi bir yapıya bağlı olmadan işlem yönetimi sağlar.
- **Servis Bağımsızlığı:** Mikroservislerin birlikte çalışmasını destekler, bağımlılığı azaltır.
- **Performans ve Ölçeklenebilirlik:** Asenkron yapı ile performansı artırır ve sistemin ölçeklenmesini kolaylaştırır.

---

## Saga Pattern: Choreography ve Orchestration Karşılaştırması

### 1. Koordinasyon Yöntemi

- **Choreography:**
  - Her servis kendi işini tamamladıktan sonra bir event yayınlar.
  - Diğer servisler bu event'i dinleyerek kendi işlemlerini başlatır.
  - Dağıtık ve olay tabanlı bir yapıdır.

- **Orchestration:**
  - Bir merkezi Orchestrator tüm işlemleri koordine eder.
  - Her servise sırayla ne yapması gerektiğini söyler.
  - Merkezi kontrol söz konusudur.

### 2. Bağımlılık

- **Choreography:**
  - Daha gevşek bağlı bir yapıya sahiptir.
  - Servisler arısı doğrudan bağımlılık azalır.

- **Orchestration:**
  - Servisler merkezi bir orchestrator'a bağımlıdır.
  - Orchestrator olmadan işlem yönetimi gerçekleşmez.

### 3. Esneklik ve Bakım

- **Choreography:**
  - Daha esnektir, yeni bir servis eklemek daha kolaydır.
  - Ancak, karmaşık bir event zinciri oluşabilir ve takip zorlaşabilir.

- **Orchestration:**
  - Merkezi bir yapı sayesinde işlem akışı daha düzenlidir.
  - Ancak, orchestrator karmaşık hale gelirse bakım zorlaşabilir.

### 4. Performans

- **Choreography:**
  - Event bazlı asenkron iletişim olduğu için daha iyi performans sunar.

- **Orchestration:**
  - Merkezi orchestrator'ın yoğunluğu performansı sınırlayabilir.

---

## Orchestration Saga Pattern: Avantajlar ve Dezavantajlar

| Kriter            | Avantajlar                                                | Dezavantajlar                                                |
|--------------------|----------------------------------------------------------|--------------------------------------------------------------|
| **Kontrol**       | Merkezi bir orchestrator işlemleri düzenler ve yönetir. | Merkezi yapı, mikroservislerin bağımsızlığını kısmen sınırlandırabilir. |
| **İzlenebilirlik** | İşlem akışını izlemek ve sorunları tespit etmek kolaydır. | Orchestrator'ın izleme yetenekleri karmaşıklaşabilir.          |
| **Hata Yönetimi**  | Hatalar ve geri alma işlemleri merkezi olarak ele alınır. | Merkezi orchestrator'ın başarısızlığı tüm sistemi etkileyebilir.  |
| **Karmaşıklık**    | Karmaşık işlem akışları ve özel mantıklar kolayca yönetilebilir. | Orchestrator'ın kendisi çok karmaşık hale gelebilir.       |
| **Bağımlılık**    | Servisler süreci merkezi olarak koordine eder.        | Servisler orchestrator'a bağımlı hale gelir.                |
| **Performans**    | İşlem akışı düzenli ve sistematik hale gelir.           | Yoğun işlem trafiğinde orchestrator darboğaz yaratabilir.  |
| **Ölçeklenebilirlik**| Küçük ve orta sistemlerde daha kolay ölçeklenebilir.      | Büyük sistemlerde orchestrator yatay ölçeklenemez.         |
| **Tek Nokta Hatası**| Sorunlar merkezi olarak çözülür.                      | Orchestrator'ın çökmesi tüm işlemleri durdurabilir.        |

---

## Saga Pattern Kullanım Senaryosu: E-Ticaret Uygulaması

Bir e-ticaret uygulamasında, müşteri bir sipariş verdiğinde şu adımların gerçekleşmesi gerekir:

1. **Stok Kontrolü:** Stokta mevcut ürünler kontrol edilir ve rezerve edilir.
2. **Ödeme Kontrolü:** Müşterinin yeterli bakiyesi olup olmadığı kontrol edilir ve ödeme gerçekleştirilir.
3. **Kargo Hazırlığı:** Ödeme onaylandıktan sonra kargo hazırlanır ve teslimat planlanır.

### Hata Durumları
- Ödeme başarısız olursa stok rezervi kaldırılmalıdır.
- Kargo işlemi başarısız olursa ödeme iade edilmelidir.

---

## Tasarım Talimatları

Saga pattern ile bu süreçteki her adımı ve hata durumunda yapılacak geri alma işlemlerini düzenleyin.

- Sipariş Verildi aşamasından Sipariş Tamamlandı aşamasına kadar olan her durumu bir durum makinesi diyagramı ile gösterin.
- Her bir başarı veya başarısızlık durumunda, sürecin hangi yöne ilerleyeceğini belirtin.

