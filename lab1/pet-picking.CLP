

;;****************
;;* DEFFUNCTIONS *
;;****************

(deffunction ask-question (?question $?allowed-values)
   (printout t ?question)
   (bind ?answer (read))
   (if (lexemep ?answer) 
       then (bind ?answer (lowcase ?answer)))
   (while (not (member$ ?answer ?allowed-values)) do
      (printout t ?question)
      (bind ?answer (read))
      (if (lexemep ?answer) 
          then (bind ?answer (lowcase ?answer))))
   ?answer)

(deffunction yes-or-no-p (?question)
   (bind ?response (ask-question ?question yes no y n))
   (if (or (eq ?response yes) (eq ?response y))
       then yes 
       else no))

;;;***************
;;;* QUERY RULES *
;;;***************

(defrule determine-free-time ""
   (not (free-time ?))
   (not (pet ?))
   =>
   (assert (free-time (yes-or-no-p "Do you have a lot of free time (yes/no)? "))))

(defrule determine-allergies ""
   (not (allergies ?))
   (not (pet ?))
   =>
   (assert (allergies (yes-or-no-p "Do you have any pet allergies (yes/no)? "))))

(defrule determine-noise-tolerance ""
   (not (noise-tolerance ?))
   (not (pet ?))
   =>
   (assert (noise-tolerance (yes-or-no-p "Do you tolerate noise (yes/no)? "))))

(defrule determine-spending ""
   (not (spending ?))
   (not (pet ?))
   =>
   (assert (spending (yes-or-no-p "Are you ready to spend a lot of money (yes/no)? "))))

(defrule determine-for-whom ""
   (not (for-whom ?))
   (not (pet ?))
   =>
   (assert (for-whom (ask-question "For whom is the pet for (me/child/office)? " me child office))))

(defrule fur-yes ""
   (and (free-time yes)      
        (allergies no))
   (not (pet ?))
   =>
   (assert (fur yes)))

(defrule fur-no ""
   (or (and (free-time no)      
            (allergies no))
       (allergies yes))
   (not (pet ?))
   =>
   (assert (fur no)))

(defrule decorative-purpose ""
   (or (and (spending yes)      
            (for-whom me))
       (for-whom office))
   (not (pet ?))
   =>
   (assert (purpose decorative)))

(defrule interactive-purpose ""
   (or (and (spending no)      
            (for-whom me))
       (for-whom child))
   (not (pet ?))
   =>
   (assert (purpose interactive)))

(defrule not-exotic-animal ""
   (or (and (spending no)      
            (purpose decorative))
       (purpose interactive))
   (not (pet ?))
   =>
   (assert (exotic no)))

(defrule exotic-animal ""
   (and (spending yes)      
        (purpose decorative))
       
   (not (pet ?))
   =>
   (assert (exotic yes)))

;;;****************
;;;* pet picking RULES *
;;;****************

(defrule pet-is-dog ""
   (and (fur yes)      
        (noise-tolerance yes)
	(exotic no))
       
   (not (pet ?))
   =>
   (assert (pet "You should get a dog.")))

(defrule pet-is-cat ""
   (and (fur yes)      
        (noise-tolerance no)
	(exotic no))
       
   (not (pet ?))
   =>
   (assert (pet "You should get a cat.")))

(defrule pet-is-fish ""
   (and (fur no)      
        (noise-tolerance no)
	(exotic no))
       
   (not (pet ?))
   =>
   (assert (pet "You should get some fishes.")))

(defrule pet-is-lizard ""
   (and (fur no)      
        (noise-tolerance no)
	(exotic yes))
       
   (not (pet ?))
   =>
   (assert (pet "You should get a lizard.")))

(defrule pet-is-lizard ""
   (and (fur no)      
        (noise-tolerance yes)
	(exotic yes))
       
   (not (pet ?))
   =>
   (assert (pet "You should get a parrot.")))

(defrule no-hope-for-you ""
  (declare (salience -10))
  (not (pet ?))
  =>
  (assert (pet "You shouldn't get a pet.")))
;;;********************************
;;;* STARTUP AND CONCLUSION RULES *
;;;********************************

(defrule system-banner ""
  (declare (salience 10))
  =>
  (printout t crlf crlf)
  (printout t "The Pet Picking Expert System")
  (printout t crlf crlf))

(defrule print-pet ""
  (declare (salience 10))
  (pet ?item)
  =>
  (printout t crlf crlf)
  (printout t "Suggested Pet:")
  (printout t crlf crlf)
  (format t " %s%n%n%n" ?item))
