variable "ResourceGroupName" {
    type        = string
    default     = "UserProfileRG"   
}

variable "ResourceGroupLocation" {
    type        = string
    default     = "uksouth"  
}

variable "sql_admin_password" {
    type      = string
	sensitive = true
}