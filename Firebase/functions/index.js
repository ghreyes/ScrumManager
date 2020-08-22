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

// Function to remove deleted users from groups
exports.userDeleted_Groups = functions.firestore.document('users/{userId}').onDelete((user, context) => {
    //DEBUG ONLY
    //db.doc('users/DeleteMe').set(user.data())
    //    .then(data => {
    //        functions.logger.log('Clone Success');
    //        return;
    //    })
    //    .catch(error => {
    //        console.error('Clone Failure - ', error);
    //        throw error;
    //    });

    var docId = context.params.userId;
    Object.keys(user.data().Groups).forEach(g => {
        try {
            functions.logger.log('Deleting user {' + docId + '} from group {' + g + '}');
            db.collection('groups').doc(g).update({ ['Users.' + docId]: admin.firestore.FieldValue.delete() });
            functions.logger.log('Deleted user {' + docId + '} from group {' + g + '}');
        }
        catch (ex) {
            functions.logger.error(ex.message);
        }
    });
    return 0;
});

// Function to update users in groups
exports.userUpdated_Groups = functions.firestore.document('users/{userId}').onUpdate((user, context) => {
    var docId = context.params.userId;
    Object.keys(user.after.data().Groups).forEach(g => {
        try {
            functions.logger.log('Updating user {' + docId + '} in group {' + g + '}');
            db.collection('groups').doc(g).update({ ['Users.' + docId + '.DisplayName']: user.after.data().DisplayName });
            functions.logger.log('Updated user {' + docId + '} in group {' + g + '}');
        }
        catch (ex) {
            functions.logger.error(ex.message);
        }
    });
    return 0;
});

// Function to remove deleted groups from users
exports.groupDeleted_Users = functions.firestore.document('groups/{groupId}').onDelete((group, context) => {
    //DEBUG ONLY
    //db.doc('groups/DeleteMe').set(group.data())
    //    .then(data => {
    //        functions.logger.log('Clone Success');
    //        return;
    //    })
    //    .catch(error => {
    //        console.error('Clone Failure - ', error);
    //        throw error;
    //    });

    var docId = context.params.groupId;
    Object.keys(group.data().Users).forEach(u => {
        try {
            functions.logger.log('Deleting group {' + docId + '} from user {' + u + '}');
            db.collection('users').doc(u).update({ ['Groups.' + docId]: admin.firestore.FieldValue.delete() });
            functions.logger.log('Deleted group {' + docId + '} from user {' + u + '}');
        }
        catch (ex) {
            functions.logger.error(ex.message);
        }
    });
    return 0;
});

// Function to update groups in users
exports.groupUpdated_Users = functions.firestore.document('groups/{groupId}').onUpdate((group, context) => {
    var docId = context.params.groupId;
    Object.keys(group.after.data().Users).forEach(u => {
        try {
            functions.logger.log('Updating group {' + docId + '} in user {' + u + '}');
            db.collection('users').doc(u).update({ ['Groups.' + docId + '.Name']: group.after.data().Data.Name });
            functions.logger.log('Updated group {' + docId + '} in user {' + u + '}');
        }
        catch (ex) {
            functions.logger.error(ex.message);
        }
    });
    return 0;
});

// Weekly PDF Job
// exports.JOBNAME = functions.pubsub.schedule('0 0 * * 0') 
// schedule('*/5 * * * *') -- every 5 mins
//exports.testJobDaily = functions.pubsub.schedule('* * 0 * *').onRun(context => {
//    //var file = new Blob(["Test Data"], { type: 'text/plain' });
//    admin.storage().bucket().file('test.txt').save("Test Data");
//    return null;
//});
