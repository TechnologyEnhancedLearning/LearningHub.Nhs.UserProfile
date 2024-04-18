variable "DNSRecordName" {
    type        = string
    description = "The name of the DNS record in the DNS Zone"
}

variable "DNSZoneResourceGroupName" {
    type        = string
    description = "The resource group which contains the DNS Zone"
}

variable "DNSZoneName" {
    type        = string
    description = "The DNS Zone Name"
}

variable "DNSRecordValue" {
    type       = string
    description = "The value for the DNS record"
}

variable "ResourceGroupName" {
    type        = string
    default     = "UserProfileRG"
}

variable "ResourceGroupLocation" {
    type        = string
    default     = "uksouth"
}
