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

//exports.createUser = functions.auth.user().onCreate((user) => {
//    db.doc('users/' + user.uid).set({ email: user.email, displayName: user.displayName });
//});
