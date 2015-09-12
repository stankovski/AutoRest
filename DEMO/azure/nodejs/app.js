var msRestAzure = require('ms-rest-azure');

var clientid = process.argv[2];
var tenantid = process.argv[3];
var secret = process.argv[4];

var ResourceManagementClient = require('./generated/resourceManagementClient')
var subscriptionCredential = new msRestAzure.SubscriptionCredentials()