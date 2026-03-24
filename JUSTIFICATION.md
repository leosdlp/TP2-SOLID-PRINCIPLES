# Justification des refactorings

## SRP — Single Responsibility Principle
Le code initial concentrait les responsabilités du réceptionniste, du comptable et de la gouvernante dans `Reservation`, et mélangeait infrastructure, métier et orchestration dans `ReservationService` et `CheckInService`. J’ai extrait un `ReservationRepository` pour l’accès aux données, un `ReservationDomainService` pour les règles métier et laissé `ReservationService` orchestrer le flux. Côté acteurs, la classe `Reservation` ne gère plus que le cycle de vie pour le réceptionniste tandis que `BillingCalculator` (comptable) et `HousekeepingScheduler` (gouvernante) assurent leurs propres règles. La maintenance s’améliore car chaque acteur peut évoluer sans impacter les autres.

## OCP — Open/Closed Principle
La politique d’annulation reposait sur un `switch` fragile. J’ai introduit l’interface `ICancellationPolicy` et des stratégies (`FlexiblePolicy`, `ModeratePolicy`, `StrictPolicy`, `NonRefundablePolicy`) injectées dans `CancellationService`. Ajouter une politique consiste désormais à fournir une nouvelle implémentation sans modifier le service, ce qui réduit les régressions et encourage l’extension. Le code contient aussi de bons exemples : `ReservationEventDispatcher` illustre l’Observer, `SeasonalSurchargeDecorator` démontre le décorateur sur `IPriceCalculator` et `ICleaningPolicy` applique le pattern Strategy pour la gouvernante.

## LSP — Liskov Substitution Principle
`NonRefundableReservation` lançait une exception via `ICancellable.Cancel()`. J’ai séparé `ICancellableReservation` de `IReservation` afin que seules les réservations annulables exposent `Cancel`. De plus, `CachedRoomRepository` respecte désormais le contrat de fraicheur en déléguant à l’implémentation interne et en invalidant correctement son cache. Ces changements garantissent que toute substitution respecte les attentes du code appelant sans surprises à l’exécution.

## ISP — Interface Segregation Principle
Plusieurs composants dépendaient d’interfaces trop larges. J’ai scindé `IReservationRepository` en interfaces focalisées (lecture, écriture, revenue, occupancy) et ajusté les consommateurs (`BillingService`, `InMemoryRoomRepository`). `InvoiceGenerator` reçoit un `InvoiceRequest` limité aux données nécessaires, et `INotificationService` est remplacé par des interfaces spécialisées (email, SMS, push, Slack). Chaque client dépend désormais du strict nécessaire, ce qui minimise les couplages.

## DIP — Dependency Inversion Principle
`BookingService` et `HousekeepingService` dépendaient directement d’implémentations techniques (`InMemoryReservationStore`, `FileLogger`, `EmailSender`). J’ai défini les abstractions `IReservationRepository`, `ILogger` et `ICleaningNotifier` dans le domaine service, puis créé des adaptateurs (`InMemoryReservationStore`, `FileLogger`, `EmailCleaningNotifier`). Les services métiers ne connaissent plus que leurs contrats et peuvent changer de stockage ou de canal de notification sans modification interne, rendant le système plus testable et extensible.
