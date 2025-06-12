@startuml
class User {
  +id: int
  +name: string
  +email: string
  +passwordHash: string
  +role: UserRole
  +registerDate: DateTime
  +editProfile()
}

class ProfessionalUser {
  +agencyId: int
  +accessAnalytics()
}

class Property {
  +id: int
  +title: string
  +description: string
  +location: string
  +price: float
  +status: PropertyStatus
  +createdDate: DateTime
}

class SearchCriteria {
  +id: int
  +keywords: string
  +minPrice: float
  +maxPrice: float
  +location: string
}

class Photo {
  +id: int
  +url: string
  +uploadedAt: DateTime
}

class Notification {
  +id: int
  +message: string
  +sentAt: DateTime
  +isRead: boolean
}

class ModerationLog {
  +id: int
  +propertyId: int
  +moderatorId: int
  +action: string
  +timestamp: DateTime
}

User <|-- ProfessionalUser
User --> "*" Property : posts
User --> "*" SearchCriteria : saves
User --> "*" Notification : receives
Property --> "*" Photo : contains
Admin --> "*" ModerationLog : records
@enduml