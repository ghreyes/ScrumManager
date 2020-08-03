// open firebase-tools app
// navigate to current folder
// 'firebase init functions' - create base files
// 'firebase deploy --only functions' - deploy any cloud functions

// The Cloud Functions for Firebase SDK to create Cloud Functions and setup triggers.
const functions = require('firebase-functions');

// The Firebase Admin SDK to access Cloud Firestore.
const admin = require('firebase-admin');
admin.initializeApp();

const db = admin.firestore();

// Function to delete any users that are deleted in Firebase
exports.deleteUser = functions.auth.user().onDelete((user) => {
    db.doc('users/' + user.uid).delete();
});

// Weekly PDF Job
// exports.JOBNAME = functions.pubsub.schedule('0 0 * * 0') 
// schedule('*/5 * * * *') -- every 5 mins
//exports.testJobDaily = functions.pubsub.schedule('* * 0 * *').onRun(context => {
//    //var file = new Blob(["Test Data"], { type: 'text/plain' });
//    admin.storage().bucket().file('test.txt').save("Test Data");
//    return null;
//});
